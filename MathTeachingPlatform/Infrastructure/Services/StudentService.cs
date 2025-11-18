using Application.DTOs.Student;
using Application.Interfaces;
using Application.Interfaces.Repositories;
using Domain.Entities;
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
        private readonly IAuthUnitOfWork _authUow;
        private readonly IContentUnitOfWork _contentUow;

        public StudentService(IAuthUnitOfWork authUow, IContentUnitOfWork contentUow)
        {
            _authUow = authUow;
            _contentUow = contentUow;
        }

        public async Task<StudentDto> CreateStudentAsync(CreateStudentRequest request)
        {
            var user = await _authUow.Users.FirstOrDefaultAsync(u => u.UserId == request.UserId);
            if (user == null)
                throw new Exception("User not found");

            if (user.Role != UserRole.Student)
                throw new Exception("User is not a student");

            var existingStudent = await _authUow.Students.FirstOrDefaultAsync(s => s.UserId == request.UserId);
            if (existingStudent != null)
                throw new Exception("Student profile already exists for this user");

            var student = new Student
            {
                UserId = request.UserId,
                Name = request.Name,
                EnrollmentDate = request.EnrollmentDate ?? DateTime.UtcNow,
                Major = request.Major,
                Status = StudentStatus.Active
            };

            await _authUow.Students.AddAsync(student);
            await _authUow.SaveChangesAsync();

            if (request.ClassId.HasValue)
            {
                var classExists = await _contentUow.Classes.AnyAsync(c => c.ClassId == request.ClassId.Value);
                if (!classExists)
                    throw new Exception("Class not found");

                var classStudent = new ClassStudent
                {
                    ClassId = request.ClassId.Value,
                    StudentId = student.StudentId,
                    EnrollmentStatus = "Active",
                    EnrolledAt = DateTime.UtcNow
                };

                await _contentUow.ClassStudents.AddAsync(classStudent);
                await _contentUow.SaveChangesAsync();
            }

            return await GetStudentByIdAsync(student.StudentId);
        }

        public async Task<StudentDto> GetStudentByIdAsync(int studentId)
        {
            var student = await _authUow.Students.FirstOrDefaultAsync(s => s.StudentId == studentId);
            if (student == null)
                throw new Exception("Student not found");

            var user = await _authUow.Users.FirstOrDefaultAsync(u => u.UserId == student.UserId);

            var enrolledClasses = await _contentUow.ClassStudents
                .Query()
                .Where(cs => cs.StudentId == studentId)
                .Include(cs => cs.Class)
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
            var student = await _authUow.Students.FirstOrDefaultAsync(s => s.UserId == userId);
            if (student == null)
                return null;

            return await GetStudentByIdAsync(student.StudentId);
        }

        public async Task<List<StudentDto>> GetAllStudentsAsync()
        {
            var students = await _authUow.Students.GetAllAsync();
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
                    continue;
                }
            }

            return result;
        }

        public async Task<List<StudentDto>> GetStudentsByClassIdAsync(int classId)
        {
            var studentIds = await _contentUow.ClassStudents
                .Query()
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
            var student = await _authUow.Students.FirstOrDefaultAsync(s => s.StudentId == studentId);
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

            _authUow.Students.Update(student);
            await _authUow.SaveChangesAsync();

            return await GetStudentByIdAsync(studentId);
        }

        public async Task<bool> SuspendStudentAsync(int studentId)
        {
            var student = await _authUow.Students.FirstOrDefaultAsync(s => s.StudentId == studentId);
            if (student == null)
                throw new Exception("Student not found");

            student.Status = StudentStatus.Suspended;
            _authUow.Students.Update(student);
            await _authUow.SaveChangesAsync();

            return true;
        }

        public async Task<bool> ActivateStudentAsync(int studentId)
        {
            var student = await _authUow.Students.FirstOrDefaultAsync(s => s.StudentId == studentId);
            if (student == null)
                throw new Exception("Student not found");

            student.Status = StudentStatus.Active;
            _authUow.Students.Update(student);
            await _authUow.SaveChangesAsync();

            return true;
        }
    }
}
