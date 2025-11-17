using System.Threading.Tasks;
using System.Collections.Generic;
using Application.DTOs.Teacher;

namespace Application.Interfaces
{
    public interface IClassApiClient
    {
        Task<bool> HasActiveClassesAsync(int teacherId);
        Task<List<ClassInfoDto>> GetClassesByTeacherIdAsync(int teacherId);
    }
}