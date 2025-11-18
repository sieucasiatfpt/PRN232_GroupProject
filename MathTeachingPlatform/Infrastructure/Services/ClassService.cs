using Application.DTOs.Class;
using Application.Interfaces;
using Application.Interfaces.Repositories;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class ClassService : IClassService
    {
        private readonly IContentUnitOfWork _contentUow;
        private readonly ITeacherApiClient _teacherApiClient;
        private readonly ISubjectApiClient _subjectApiClient;
        private readonly IStudentApiClient _studentApiClient;

        public ClassService(
            IContentUnitOfWork contentUow,
            ITeacherApiClient teacherApiClient,
            ISubjectApiClient subjectApiClient,
            IStudentApiClient studentApiClient)
        {
            _contentUow = contentUow;
            _teacherApiClient = teacherApiClient;
            _subjectApiClient = subjectApiClient;
            _studentApiClient = studentApiClient;
        }

        public async Task<ClassDto> CreateClassAsync(CreateClassRequest request)
        {
            // Business Rule: Validate start date is not in the past
            if (request.StartDate.HasValue && request.StartDate.Value.Date < DateTime.UtcNow.Date)
            {
                throw new InvalidOperationException($"Class start date cannot be in the past. Provided date: {request.StartDate.Value:yyyy-MM-dd}, Current date: {DateTime.UtcNow:yyyy-MM-dd}");
            }

            // Business Rule: Validate end date is after start date
            if (request.StartDate.HasValue && request.EndDate.HasValue && request.EndDate.Value <= request.StartDate.Value)
            {
                throw new InvalidOperationException($"Class end date must be after the start date. Start date: {request.StartDate.Value:yyyy-MM-dd}, End date: {request.EndDate.Value:yyyy-MM-dd}");
            }

            // Check if subject exists via API call
            var subjectExists = await _subjectApiClient.SubjectExistsAsync(request.SubjectId);
            if (!subjectExists)
                throw new Exception("Subject not found");

            // Check if teacher exists via API call
            var teacherExists = await _teacherApiClient.TeacherExistsAsync(request.TeacherId);
            if (!teacherExists)
                throw new Exception("Teacher not found");

            // Business Rule: Check if teacher already has a class with the same subject
            var existingClassWithSameSubject = await _contentUow.Classes
                .FirstOrDefaultAsync(c => c.TeacherId == request.TeacherId &&
                                        c.SubjectId == request.SubjectId);

            if (existingClassWithSameSubject != null)
            {
                throw new InvalidOperationException($"Teacher is already assigned to class '{existingClassWithSameSubject.Name}' (ID: {existingClassWithSameSubject.ClassId}) with the same subject (Subject ID: {request.SubjectId}). A teacher cannot have multiple classes for the same subject.");
            }

            // Business Rule: Check if teacher already has a class starting on the same day
            if (request.StartDate.HasValue)
            {
                var teacherClassOnSameDay = await _contentUow.Classes
                    .FirstOrDefaultAsync(c => c.TeacherId == request.TeacherId &&
                                            c.StartDate.HasValue &&
                                            c.StartDate.Value.Date == request.StartDate.Value.Date);

                if (teacherClassOnSameDay != null)
                {
                    throw new InvalidOperationException($"Teacher is already assigned to class '{teacherClassOnSameDay.Name}' (ID: {teacherClassOnSameDay.ClassId}) that starts on {request.StartDate.Value:yyyy-MM-dd}. A teacher cannot have multiple classes starting on the same day.");
                }
            }

            var classEntity = new Class
            {
                SubjectId = request.SubjectId,
                TeacherId = request.TeacherId,
                Name = request.Name,
                Schedule = request.Schedule,
                StartDate = request.StartDate,
                EndDate = request.EndDate,
                CreatedAt = DateTime.UtcNow
            };

            await _contentUow.Classes.AddAsync(classEntity);
            await _contentUow.SaveChangesAsync();

            return await GetClassByIdAsync(classEntity.ClassId);
        }

        public async Task<ClassDto> GetClassByIdAsync(int classId)
        {
            var classEntity = await _contentUow.Classes.FirstOrDefaultAsync(c => c.ClassId == classId);
            if (classEntity == null)
                throw new Exception("Class not found");

            // Get subject and teacher info via API calls
            var subjectTitle = await _subjectApiClient.GetSubjectTitleAsync(classEntity.SubjectId);
            var teacherName = await _teacherApiClient.GetTeacherNameAsync(classEntity.TeacherId);

            var enrolledStudents = await _contentUow.ClassStudents
                .Query()
                .Where(cs => cs.ClassId == classId)
                .ToListAsync();

            var studentInfos = new List<StudentInfoDto>();
            foreach (var enrollment in enrolledStudents)
            {
                var studentName = await _studentApiClient.GetStudentNameAsync(enrollment.StudentId);
                if (studentName != null)
                {
                    studentInfos.Add(new StudentInfoDto
                    {
                        StudentId = enrollment.StudentId,
                        Name = studentName,
                        EnrollmentStatus = enrollment.EnrollmentStatus,
                        EnrolledAt = enrollment.EnrolledAt
                    });
                }
            }

            return new ClassDto
            {
                ClassId = classEntity.ClassId,
                SubjectId = classEntity.SubjectId,
                TeacherId = classEntity.TeacherId,
                Name = classEntity.Name,
                Schedule = classEntity.Schedule,
                StartDate = classEntity.StartDate,
                EndDate = classEntity.EndDate,
                CreatedAt = classEntity.CreatedAt,
                SubjectTitle = subjectTitle,
                TeacherName = teacherName,
                EnrolledStudents = studentInfos
            };
        }

        public async Task<List<ClassDto>> GetAllClassesAsync()
        {
            var classes = await _contentUow.Classes.GetAllAsync();
            var result = new List<ClassDto>();

            foreach (var classEntity in classes)
            {
                try
                {
                    var dto = await GetClassByIdAsync(classEntity.ClassId);
                    result.Add(dto);
                }
                catch
                {
                    continue;
                }
            }

            return result;
        }

        public async Task<List<ClassDto>> GetClassesByTeacherIdAsync(int teacherId)
        {
            var classes = await _contentUow.Classes.FindAsync(c => c.TeacherId == teacherId);
            var result = new List<ClassDto>();

            foreach (var classEntity in classes)
            {
                try
                {
                    var dto = await GetClassByIdAsync(classEntity.ClassId);
                    result.Add(dto);
                }
                catch
                {
                    continue;
                }
            }

            return result;
        }

        public async Task<List<ClassDto>> GetClassesBySubjectIdAsync(int subjectId)
        {
            var classes = await _contentUow.Classes.FindAsync(c => c.SubjectId == subjectId);
            var result = new List<ClassDto>();

            foreach (var classEntity in classes)
            {
                try
                {
                    var dto = await GetClassByIdAsync(classEntity.ClassId);
                    result.Add(dto);
                }
                catch
                {
                    continue;
                }
            }

            return result;
        }

        public async Task<ClassDto> UpdateClassAsync(int classId, UpdateClassRequest request)
        {
            var classEntity = await _contentUow.Classes.FirstOrDefaultAsync(c => c.ClassId == classId);
            if (classEntity == null)
                throw new Exception("Class not found");

            // Business Rule: Validate start date is not in the past
            if (request.StartDate.HasValue && request.StartDate.Value.Date < DateTime.UtcNow.Date)
            {
                throw new InvalidOperationException($"Class start date cannot be set to a past date. Provided date: {request.StartDate.Value:yyyy-MM-dd}, Current date: {DateTime.UtcNow:yyyy-MM-dd}");
            }

            // Business Rule: Validate end date is after start date
            var newStartDate = request.StartDate ?? classEntity.StartDate;
            if (request.EndDate.HasValue && newStartDate.HasValue && request.EndDate.Value <= newStartDate.Value)
            {
                throw new InvalidOperationException($"Class end date must be after the start date. Start date: {newStartDate.Value:yyyy-MM-dd}, End date: {request.EndDate.Value:yyyy-MM-dd}");
            }

            if (!string.IsNullOrWhiteSpace(request.Name))
                classEntity.Name = request.Name;

            if (request.Schedule != null)
                classEntity.Schedule = request.Schedule;

            if (request.StartDate.HasValue)
                classEntity.StartDate = request.StartDate;

            if (request.EndDate.HasValue)
                classEntity.EndDate = request.EndDate;

            if (request.SubjectId.HasValue)
            {
                var subjectExists = await _subjectApiClient.SubjectExistsAsync(request.SubjectId.Value);
                if (!subjectExists)
                    throw new Exception("Subject not found");

                classEntity.SubjectId = request.SubjectId.Value;
            }

            if (request.TeacherId.HasValue)
            {
                var teacherExists = await _teacherApiClient.TeacherExistsAsync(request.TeacherId.Value);
                if (!teacherExists)
                    throw new Exception("Teacher not found");

                // Business Rule: Check if new teacher already has a class with the same subject
                if (request.SubjectId.HasValue || classEntity.SubjectId > 0)
                {
                    var subjectIdToCheck = request.SubjectId ?? classEntity.SubjectId;
                    var existingClassWithSameSubject = await _contentUow.Classes
                        .FirstOrDefaultAsync(c => c.TeacherId == request.TeacherId.Value &&
                                                c.SubjectId == subjectIdToCheck &&
                                                c.ClassId != classId);

                    if (existingClassWithSameSubject != null)
                    {
                        throw new InvalidOperationException($"The new teacher is already assigned to class '{existingClassWithSameSubject.Name}' (ID: {existingClassWithSameSubject.ClassId}) with the same subject (Subject ID: {subjectIdToCheck}). A teacher cannot have multiple classes for the same subject.");
                    }
                }

                classEntity.TeacherId = request.TeacherId.Value;
            }

            _contentUow.Classes.Update(classEntity);
            await _contentUow.SaveChangesAsync();

            return await GetClassByIdAsync(classId);
        }

        public async Task<bool> DeleteClassAsync(int classId)
        {
            var classEntity = await _contentUow.Classes.FirstOrDefaultAsync(c => c.ClassId == classId);
            if (classEntity == null)
                throw new Exception("Class not found");

            var enrollments = await _contentUow.ClassStudents.FindAsync(cs => cs.ClassId == classId);
            _contentUow.ClassStudents.RemoveRange(enrollments);

            var activities = await _contentUow.Activities.FindAsync(a => a.ClassId == classId);
            _contentUow.Activities.RemoveRange(activities);

            _contentUow.Classes.Remove(classEntity);
            await _contentUow.SaveChangesAsync();

            return true;
        }

        public async Task<bool> EnrollStudentAsync(int classId, int studentId)
        {
            var classExists = await _contentUow.Classes.AnyAsync(c => c.ClassId == classId);
            if (!classExists)
                throw new Exception("Class not found");

            // Check if student exists via API call
            var studentExists = await _studentApiClient.StudentExistsAsync(studentId);
            if (!studentExists)
                throw new Exception("Student not found");

            var existingEnrollment = await _contentUow.ClassStudents
                .FirstOrDefaultAsync(cs => cs.ClassId == classId && cs.StudentId == studentId);
            
            if (existingEnrollment != null)
                throw new Exception("Student is already enrolled in this class");

            var enrollment = new ClassStudent
            {
                ClassId = classId,
                StudentId = studentId,
                EnrollmentStatus = "Active",
                EnrolledAt = DateTime.UtcNow
            };

            await _contentUow.ClassStudents.AddAsync(enrollment);
            await _contentUow.SaveChangesAsync();

            return true;
        }

        public async Task<bool> UnenrollStudentAsync(int classId, int studentId)
        {
            var enrollment = await _contentUow.ClassStudents
                .FirstOrDefaultAsync(cs => cs.ClassId == classId && cs.StudentId == studentId);

            if (enrollment == null)
                throw new Exception("Student enrollment not found");

            _contentUow.ClassStudents.Remove(enrollment);
            await _contentUow.SaveChangesAsync();

            return true;
        }
        public async Task<bool> HasActiveClassesAsync(int teacherId)
        {
            var activeClasses = await _contentUow.Classes
                .AnyAsync(c => c.TeacherId == teacherId &&
                              (c.EndDate == null || c.EndDate > DateTime.UtcNow));
            return activeClasses;
        }
    }
}