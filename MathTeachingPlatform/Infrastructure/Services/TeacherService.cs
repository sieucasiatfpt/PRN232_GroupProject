using Application.DTOs.Teacher;
using Application.Interfaces;
using Application.Interfaces.Repositories;
using Domain.Entities;
using Domain.Enum;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class TeacherService : ITeacherService
    {
        private readonly IAuthUnitOfWork _authUow;
        private readonly ISubjectApiClient _subjectApiClient;
        private readonly IClassApiClient _classApiClient;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public TeacherService(
            IAuthUnitOfWork authUow,
            ISubjectApiClient subjectApiClient,
            IClassApiClient classApiClient,
            IHttpContextAccessor httpContextAccessor)
        {
            _authUow = authUow;
            _subjectApiClient = subjectApiClient;
            _classApiClient = classApiClient;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<TeacherDto> CreateTeacherAsync(CreateTeacherRequest request)
        {
            // Check if the user exists
            var user = await _authUow.Users.FirstOrDefaultAsync(u => u.UserId == request.UserId);
            if (user == null)
                throw new Exception("User not found");

            // Check if the user is already associated with a student
            var isStudent = await _authUow.Students.AnyAsync(s => s.UserId == request.UserId);
            if (isStudent)
                throw new Exception("Cannot create a teacher profile for a user who is already a student");

            // Check if the user has the Teacher role
            if (user.Role != UserRole.Teacher)
                throw new Exception("User is not a teacher");

            // Check if a teacher profile already exists for this user
            var existingTeacher = await _authUow.Teachers.FirstOrDefaultAsync(t => t.UserId == request.UserId);
            if (existingTeacher != null)
                throw new Exception("Teacher profile already exists for this user");

            // Create the teacher profile
            var teacher = new Teacher
            {
                UserId = request.UserId,
                Name = request.Name,
                Bio = request.Bio,
                HireDate = request.HireDate ?? DateTime.UtcNow,
                Department = request.Department,
                Status = TeacherStatus.Active
            };

            await _authUow.Teachers.AddAsync(teacher);
            await _authUow.SaveChangesAsync();

            return await GetTeacherByIdAsync(teacher.TeacherId);
        }

        public async Task<TeacherDto> GetTeacherByIdAsync(int teacherId)
        {
            var teacher = await _authUow.Teachers.FirstOrDefaultAsync(t => t.TeacherId == teacherId);
            if (teacher == null)
                throw new Exception("Teacher not found");

            var user = await _authUow.Users.FirstOrDefaultAsync(u => u.UserId == teacher.UserId);

            // Get subjects from Subject service via API
            var subjects = new List<SubjectInfoDto>();
            try
            {
                // This would typically be a call to get subjects by teacher ID
                // For now, we'll keep it empty as we don't have direct access to content database
            }
            catch
            {
                // If Subject service is unavailable, continue without subjects
            }

            // Get classes from Class service via API
            var classes = new List<ClassInfoDto>();
            try
            {
                // This would typically be a call to get classes by teacher ID
                // For now, we'll keep it empty as we don't have direct access to content database
            }
            catch
            {
                // If Class service is unavailable, continue without classes
            }

            return new TeacherDto
            {
                TeacherId = teacher.TeacherId,
                UserId = teacher.UserId,
                Name = teacher.Name,
                Username = user?.Username,
                Email = user?.Email,
                Bio = teacher.Bio,
                HireDate = teacher.HireDate,
                Department = teacher.Department,
                Status = teacher.Status,
                Subjects = subjects,
                Classes = classes
            };
        }

        public async Task<TeacherDto?> GetTeacherByUserIdAsync(int userId)
        {
            var teacher = await _authUow.Teachers.FirstOrDefaultAsync(t => t.UserId == userId);
            if (teacher == null)
                return null;

            return await GetTeacherByIdAsync(teacher.TeacherId);
        }

        public async Task<List<TeacherDto>> GetAllTeachersAsync()
        {
            var teachers = await _authUow.Teachers.GetAllAsync();
            var result = new List<TeacherDto>();

            foreach (var teacher in teachers)
            {
                try
                {
                    var dto = await GetTeacherByIdAsync(teacher.TeacherId);
                    result.Add(dto);
                }
                catch
                {
                    continue;
                }
            }

            return result;
        }

        public async Task<List<TeacherDto>> GetTeachersByDepartmentAsync(string department)
        {
            var teachers = await _authUow.Teachers.FindAsync(t => t.Department == department);
            var result = new List<TeacherDto>();

            foreach (var teacher in teachers)
            {
                try
                {
                    var dto = await GetTeacherByIdAsync(teacher.TeacherId);
                    result.Add(dto);
                }
                catch
                {
                    continue;
                }
            }

            return result;
        }

        public async Task<TeacherDto> UpdateTeacherAsync(int teacherId, UpdateTeacherRequest request)
        {
            var teacher = await _authUow.Teachers.FirstOrDefaultAsync(t => t.TeacherId == teacherId);
            if (teacher == null)
                throw new Exception("Teacher not found");

            if (!string.IsNullOrWhiteSpace(request.Name))
                teacher.Name = request.Name;

            if (request.Bio != null)
                teacher.Bio = request.Bio;

            if (request.HireDate.HasValue)
                teacher.HireDate = request.HireDate;

            if (!string.IsNullOrWhiteSpace(request.Department))
                teacher.Department = request.Department;

            if (request.Status.HasValue)
                teacher.Status = request.Status.Value;

            _authUow.Teachers.Update(teacher);
            await _authUow.SaveChangesAsync();

            return await GetTeacherByIdAsync(teacherId);
        }

        public async Task<bool> DeleteTeacherAsync(int teacherId)
        {
            var teacher = await _authUow.Teachers.FirstOrDefaultAsync(t => t.TeacherId == teacherId);
            if (teacher == null)
                throw new Exception("Teacher not found");

            // Note: In microservices architecture, you might need to check with other services
            // before allowing deletion, or implement eventual consistency patterns

            _authUow.Teachers.Remove(teacher);
            await _authUow.SaveChangesAsync();

            return true;
        }

        public async Task<bool> SuspendTeacherAsync(int teacherId)
        {
            var teacher = await _authUow.Teachers.FirstOrDefaultAsync(t => t.TeacherId == teacherId);
            if (teacher == null)
                throw new Exception("Teacher not found");

            // Get JWT token and set it in ClassApiClient
            var jwtToken = GetJwtToken();
            if (!string.IsNullOrEmpty(jwtToken))
            {
                _classApiClient.SetToken(jwtToken);
            }

            // Check if the teacher has any active classes
            var hasActiveClasses = await _classApiClient.HasActiveClassesAsync(teacherId);
            if (hasActiveClasses)
                throw new Exception("Cannot suspend teacher with active classes");

            teacher.Status = TeacherStatus.Inactive;
            _authUow.Teachers.Update(teacher);
            await _authUow.SaveChangesAsync();

            return true;
        }

        public async Task<bool> ActivateTeacherAsync(int teacherId)
        {
            var teacher = await _authUow.Teachers.FirstOrDefaultAsync(t => t.TeacherId == teacherId);
            if (teacher == null)
                throw new Exception("Teacher not found");

            teacher.Status = TeacherStatus.Active;
            _authUow.Teachers.Update(teacher);
            await _authUow.SaveChangesAsync();

            return true;
        }

        private string? GetJwtToken()
        {
            var authHeader = _httpContextAccessor.HttpContext?.Request.Headers["Authorization"].ToString();
            if (!string.IsNullOrEmpty(authHeader) && authHeader.StartsWith("Bearer "))
            {
                return authHeader.Substring("Bearer ".Length).Trim();
            }
            return null;
        }
    }
}