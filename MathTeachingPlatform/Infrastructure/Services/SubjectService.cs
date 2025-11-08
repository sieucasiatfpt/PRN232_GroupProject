using Application.DTOs.Subject;
using Application.DTOs.Teacher;
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
    public class SubjectService : ISubjectService
    {
        private readonly IContentUnitOfWork _contentUow;
        private readonly ITeacherApiClient _teacherApiClient;

        public SubjectService(IContentUnitOfWork contentUow, ITeacherApiClient teacherApiClient)
        {
            _contentUow = contentUow;
            _teacherApiClient = teacherApiClient;
        }

        public async Task<SubjectDto> CreateSubjectAsync(CreateSubjectRequest request)
        {
            // Check if teacher exists via API call
            var teacherExists = await _teacherApiClient.TeacherExistsAsync(request.TeacherId);
            if (!teacherExists)
                throw new Exception("Teacher not found");

            var subject = new Subject
            {
                TeacherId = request.TeacherId,
                Title = request.Title,
                Description = request.Description,
                CreatedAt = DateTime.UtcNow
            };

            await _contentUow.Subjects.AddAsync(subject);
            await _contentUow.SaveChangesAsync();

            return await GetSubjectByIdAsync(subject.SubjectId);
        }

        public async Task<SubjectDto> GetSubjectByIdAsync(int subjectId)
        {
            var subject = await _contentUow.Subjects.FirstOrDefaultAsync(s => s.SubjectId == subjectId);
            if (subject == null)
                throw new Exception("Subject not found");

            // Get teacher name via API call
            var teacherName = await _teacherApiClient.GetTeacherNameAsync(subject.TeacherId);

            var classes = await _contentUow.Classes
                .Query()
                .Where(c => c.SubjectId == subjectId)
                .ToListAsync();

            return new SubjectDto
            {
                SubjectId = subject.SubjectId,
                TeacherId = subject.TeacherId,
                Title = subject.Title,
                Description = subject.Description,
                CreatedAt = subject.CreatedAt,
                UpdatedAt = subject.UpdatedAt,
                TeacherName = teacherName,
                Classes = classes.Select(c => new ClassInfoDto
                {
                    ClassId = c.ClassId,
                    Name = c.Name,
                    Schedule = c.Schedule,
                    StartDate = c.StartDate,
                    EndDate = c.EndDate
                }).ToList()
            };
        }

        public async Task<List<SubjectDto>> GetAllSubjectsAsync()
        {
            var subjects = await _contentUow.Subjects.GetAllAsync();
            var result = new List<SubjectDto>();

            foreach (var subject in subjects)
            {
                try
                {
                    var dto = await GetSubjectByIdAsync(subject.SubjectId);
                    result.Add(dto);
                }
                catch
                {
                    continue;
                }
            }

            return result;
        }

        public async Task<List<SubjectDto>> GetSubjectsByTeacherIdAsync(int teacherId)
        {
            var subjects = await _contentUow.Subjects.FindAsync(s => s.TeacherId == teacherId);
            var result = new List<SubjectDto>();

            foreach (var subject in subjects)
            {
                try
                {
                    var dto = await GetSubjectByIdAsync(subject.SubjectId);
                    result.Add(dto);
                }
                catch
                {
                    continue;
                }
            }

            return result;
        }

        public async Task<SubjectDto> UpdateSubjectAsync(int subjectId, UpdateSubjectRequest request)
        {
            var subject = await _contentUow.Subjects.FirstOrDefaultAsync(s => s.SubjectId == subjectId);
            if (subject == null)
                throw new Exception("Subject not found");

            if (!string.IsNullOrWhiteSpace(request.Title))
                subject.Title = request.Title;

            if (request.Description != null)
                subject.Description = request.Description;

            if (request.TeacherId.HasValue)
            {
                var teacherExists = await _teacherApiClient.TeacherExistsAsync(request.TeacherId.Value);
                if (!teacherExists)
                    throw new Exception("Teacher not found");

                subject.TeacherId = request.TeacherId.Value;
            }

            subject.UpdatedAt = DateTime.UtcNow;

            _contentUow.Subjects.Update(subject);
            await _contentUow.SaveChangesAsync();

            return await GetSubjectByIdAsync(subjectId);
        }

        public async Task<bool> DeleteSubjectAsync(int subjectId)
        {
            var subject = await _contentUow.Subjects.FirstOrDefaultAsync(s => s.SubjectId == subjectId);
            if (subject == null)
                throw new Exception("Subject not found");

            var hasClasses = await _contentUow.Classes.AnyAsync(c => c.SubjectId == subjectId);
            if (hasClasses)
                throw new Exception("Cannot delete subject with associated classes");

            _contentUow.Subjects.Remove(subject);
            await _contentUow.SaveChangesAsync();

            return true;
        }
    }
}