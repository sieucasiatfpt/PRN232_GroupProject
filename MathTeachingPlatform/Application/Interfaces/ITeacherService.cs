using Application.DTOs.Teacher;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface ITeacherService
    {
        Task<TeacherDto> CreateTeacherAsync(CreateTeacherRequest request);
        Task<TeacherDto> GetTeacherByIdAsync(int teacherId);
        Task<TeacherDto?> GetTeacherByUserIdAsync(int userId);
        Task<List<TeacherDto>> GetAllTeachersAsync();
        Task<TeacherDto> UpdateTeacherAsync(int teacherId, UpdateTeacherRequest request);
        Task<bool> DeleteTeacherAsync(int teacherId);
        Task<bool> SuspendTeacherAsync(int teacherId);
        Task<bool> ActivateTeacherAsync(int teacherId);
        Task<List<TeacherDto>> GetTeachersByDepartmentAsync(string department);
    }
}