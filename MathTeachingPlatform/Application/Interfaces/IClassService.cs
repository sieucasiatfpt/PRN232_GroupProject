using Application.DTOs.Class;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IClassService
    {
        Task<ClassDto> CreateClassAsync(CreateClassRequest request);
        Task<ClassDto> GetClassByIdAsync(int classId);
        Task<List<ClassDto>> GetAllClassesAsync();
        Task<List<ClassDto>> GetClassesByTeacherIdAsync(int teacherId);
        Task<List<ClassDto>> GetClassesBySubjectIdAsync(int subjectId);
        Task<ClassDto> UpdateClassAsync(int classId, UpdateClassRequest request);
        Task<bool> DeleteClassAsync(int classId);
        Task<bool> EnrollStudentAsync(int classId, int studentId);
        Task<bool> UnenrollStudentAsync(int classId, int studentId);
    }
}