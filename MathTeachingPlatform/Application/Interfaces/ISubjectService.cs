using Application.DTOs.Subject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface ISubjectService
    {
        Task<SubjectDto> CreateSubjectAsync(CreateSubjectRequest request);
        Task<SubjectDto> GetSubjectByIdAsync(int subjectId);
        Task<List<SubjectDto>> GetAllSubjectsAsync();
        Task<List<SubjectDto>> GetSubjectsByTeacherIdAsync(int teacherId);
        Task<SubjectDto> UpdateSubjectAsync(int subjectId, UpdateSubjectRequest request);
        Task<bool> DeleteSubjectAsync(int subjectId);
    }
}