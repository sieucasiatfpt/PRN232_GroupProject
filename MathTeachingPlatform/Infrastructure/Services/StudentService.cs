using Application.DTOs.Student;
using Application.Interfaces;
using Domain.Enum;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class StudentService : IStudentService
    {
        private readonly AuthDbContext _authDb;
        private readonly ContentDbContext _contentDb;

        public StudentService(AuthDbContext authDb, ContentDbContext contentDb)
        {
            _authDb = authDb;
            _contentDb = contentDb;
        }

        public async Task<StudentDto> CreateStudentAsync(CreateStudentRequest request)
        {
            // Check if user exists
            var user = await _authDb.Users.FirstOrDefaultAsync(u => u.UserId == request.UserId);
            if (user == null)
                throw new Exception("User not found");

            // Check if user is a student
            if (user.Role != UserRole.Student)
                throw new Exception("User is not a student");

            // Check if student already exists for this user
            var existingStudent = await _authDb.Students.FirstOrDefaultAsync(s => s.UserId == request.UserId);
            if (existingStudent != null)
                throw new Exception("Student profile already exists for this user");

            var student = new Domain.Entities.Student
            {
                UserId = request.UserId,
                Name = request.Name,
                EnrollmentDate = request.EnrollmentDate ?? DateTime.UtcNow,
                Major = request.Major,
                Status = StudentStatus.Active
            };

            _authDb.Students.Add(student);
            await _authDb.SaveChangesAsync();

            // If ClassId is provided, enroll the student in that class
            if (request.ClassId.HasValue)
            {
                var classExists = await _contentDb.Classes.AnyAsync(c => c.ClassId == request.ClassId.Value);
                if (!classExists)
                    throw new Exception("Class not found");

                var classStudent = new Domain.Entities.ClassStudent
                {
                    ClassId = request.ClassId.Value,
                    StudentId = student.StudentId,
                    EnrollmentStatus = "Active",
                    EnrolledAt = DateTime.UtcNow
                };

                _contentDb.ClassStudents.Add(classStudent);
                await _contentDb.SaveChangesAsync();
            }

            return await GetStudentByIdAsync(student.StudentId);
        }

        public async Task<StudentDto> GetStudentByIdAsync(int studentId)
        {
            var student = await _authDb.Students.FirstOrDefaultAsync(s => s.StudentId == studentId);
            if (student == null)
                throw new Exception("Student not found");

            var user = await _authDb.Users.FirstOrDefaultAsync(u => u.UserId == student.UserId);

            // Get all classes the student is enrolled in
            var enrolledClasses = await _contentDb.ClassStudents
                .Where(cs => cs.StudentId == studentId)
                .Include(cs => cs.ClassId)
                .Select(cs => new { cs.ClassId, cs.Class.Name })
                .ToListAsync();

            return new StudentDto
            {
                StudentId = student.StudentId,
                UserId = student.UserId,
                Name = student.Name,
                Username = user?.Username,
                Email = user?.Email,
                Major = student.Major,
                EnrolledClasses = enrolledClasses.Select(c => new ClassInfoDto
                {
                    ClassId = c.ClassId,
                    ClassName = c.Name
                }).ToList(),
                EnrollmentDate = student.EnrollmentDate,
                Status = student.Status
            };
        }

        public async Task<StudentDto?> GetStudentByUserIdAsync(int userId)
        {
            var student = await _authDb.Students.FirstOrDefaultAsync(s => s.UserId == userId);
            if (student == null)
                return null;

            return await GetStudentByIdAsync(student.StudentId);
        }

        public async Task<List<StudentDto>> GetAllStudentsAsync()
        {
            var students = await _authDb.Students.ToListAsync();
            var result = new List<StudentDto>();

            foreach (var student in students)
            {
                try
                {
                    var dto = await GetStudentByIdAsync(student.StudentId);
                    result.Add(dto);
                }
                catch
                {
                    // Skip students with errors
                    continue;
                }
            }

            return result;
        }

        public async Task<List<StudentDto>> GetStudentsByClassIdAsync(int classId)
        {
            // Get student IDs enrolled in the class through ClassStudent join table
            var studentIds = await _contentDb.ClassStudents
                .Where(cs => cs.ClassId == classId && cs.EnrollmentStatus == "Active")
                .Select(cs => cs.StudentId)
                .ToListAsync();

            var result = new List<StudentDto>();

            foreach (var studentId in studentIds)
            {
                try
                {
                    var dto = await GetStudentByIdAsync(studentId);
                    result.Add(dto);
                }
                catch
                {
                    continue;
                }
            }

            return result;
        }

        public async Task<StudentDto> UpdateStudentAsync(int studentId, UpdateStudentRequest request)
        {
            var student = await _authDb.Students.FirstOrDefaultAsync(s => s.StudentId == studentId);
            if (student == null)
                throw new Exception("Student not found");

            if (!string.IsNullOrWhiteSpace(request.Name))
                student.Name = request.Name;

            if (!string.IsNullOrWhiteSpace(request.Major))
                student.Major = request.Major;

            if (request.Status.HasValue)
                student.Status = request.Status.Value;

            if (request.EnrollmentDate.HasValue)
                student.EnrollmentDate = request.EnrollmentDate.Value;

            await _authDb.SaveChangesAsync();

            return await GetStudentByIdAsync(studentId);
        }

        public async Task<bool> EnrollStudentInClassAsync(int studentId, int classId)
        {
            // Verify student exists
            var studentExists = await _authDb.Students.AnyAsync(s => s.StudentId == studentId);
            if (!studentExists)
                throw new Exception("Student not found");

            // Verify class exists
            var classExists = await _contentDb.Classes.AnyAsync(c => c.ClassId == classId);
            if (!classExists)
                throw new Exception("Class not found");

            // Check if already enrolled
            var alreadyEnrolled = await _contentDb.ClassStudents
                .AnyAsync(cs => cs.StudentId == studentId && cs.ClassId == classId);

            if (alreadyEnrolled)
                throw new Exception("Student is already enrolled in this class");

            var classStudent = new Domain.Entities.ClassStudent
            {
                ClassId = classId,
                StudentId = studentId,
                EnrollmentStatus = "Active",
                EnrolledAt = DateTime.UtcNow
            };

            _contentDb.ClassStudents.Add(classStudent);
            await _contentDb.SaveChangesAsync();

            return true;
        }

        public async Task<bool> UnenrollStudentFromClassAsync(int studentId, int classId)
        {
            var classStudent = await _contentDb.ClassStudents
                .FirstOrDefaultAsync(cs => cs.StudentId == studentId && cs.ClassId == classId);

            if (classStudent == null)
                throw new Exception("Enrollment not found");

            _contentDb.ClassStudents.Remove(classStudent);
            await _contentDb.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteStudentAsync(int studentId)
        {
            var student = await _authDb.Students.FirstOrDefaultAsync(s => s.StudentId == studentId);
            if (student == null)
                throw new Exception("Student not found");

            // Remove all class enrollments first
            var enrollments = await _contentDb.ClassStudents
                .Where(cs => cs.StudentId == studentId)
                .ToListAsync();

            _contentDb.ClassStudents.RemoveRange(enrollments);
            await _contentDb.SaveChangesAsync();

            // Remove student
            _authDb.Students.Remove(student);
            await _authDb.SaveChangesAsync();

            return true;
        }

        public async Task<bool> SuspendStudentAsync(int studentId)
        {
            var student = await _authDb.Students.FirstOrDefaultAsync(s => s.StudentId == studentId);
            if (student == null)
                throw new Exception("Student not found");

            student.Status = StudentStatus.Suspended;
            await _authDb.SaveChangesAsync();

            return true;
        }

        public async Task<bool> ActivateStudentAsync(int studentId)
        {
            var student = await _authDb.Students.FirstOrDefaultAsync(s => s.StudentId == studentId);
            if (student == null)
                throw new Exception("Student not found");

            student.Status = StudentStatus.Active;
            await _authDb.SaveChangesAsync();

            return true;
        }
    }
}
