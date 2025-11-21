using Application.DTOs.Student;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IStudentApiClient
    {
        Task<StudentDto?> GetStudentByIdAsync(int studentId);
        Task<bool> StudentExistsAsync(int studentId);
        Task<string?> GetStudentNameAsync(int studentId);
    }
}