using Application.DTOs.Student;
using Domain.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IStudentService
    {
        Task<StudentDto> CreateStudentAsync(CreateStudentRequest request);
        Task<StudentDto> GetStudentByIdAsync(int studentId);
        Task<StudentDto?> GetStudentByUserIdAsync(int userId);
        Task<List<StudentDto>> GetAllStudentsAsync();
        Task<List<StudentDto>> GetStudentsByClassIdAsync(int classId);
        Task<StudentDto> UpdateStudentAsync(int studentId, UpdateStudentRequest request);
        Task<bool> SuspendStudentAsync(int studentId);
        Task<bool> ActivateStudentAsync(int studentId);
        Task<StudentDto> UpdateStudentStatusAsync(int studentId, StudentStatus status);
    }
}
