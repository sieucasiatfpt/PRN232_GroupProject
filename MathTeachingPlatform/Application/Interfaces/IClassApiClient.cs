using System.Threading.Tasks;
using System.Collections.Generic;
using Application.DTOs.Teacher;

namespace Application.Interfaces
{
    public interface IClassApiClient
    {
        void SetToken(string jwtToken);
        Task<bool> HasActiveClassesAsync(int teacherId);
        Task<List<ClassInfoDto>> GetClassesByTeacherIdAsync(int teacherId);
    }
}