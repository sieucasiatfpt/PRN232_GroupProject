using Application.DTOs.Subject;
using Application.DTOs.Teacher;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface ISubjectApiClient
    {
        Task<SubjectDto?> GetSubjectByIdAsync(int subjectId);
        Task<bool> SubjectExistsAsync(int subjectId);
        Task<string?> GetSubjectTitleAsync(int subjectId);
        Task<List<SubjectInfoDto>> GetSubjectsByTeacherIdAsync(int teacherId);
    }
}