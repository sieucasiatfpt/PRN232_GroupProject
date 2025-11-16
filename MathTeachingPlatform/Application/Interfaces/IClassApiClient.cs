using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IClassApiClient
    {
        Task<bool> HasActiveClassesAsync(int teacherId);
    }
}