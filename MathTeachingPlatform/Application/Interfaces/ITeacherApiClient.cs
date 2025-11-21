using Application.DTOs.Teacher;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface ITeacherApiClient
    {
        Task<TeacherDto?> GetTeacherByIdAsync(int teacherId);
        Task<bool> TeacherExistsAsync(int teacherId);
        Task<string?> GetTeacherNameAsync(int teacherId);
    }
}