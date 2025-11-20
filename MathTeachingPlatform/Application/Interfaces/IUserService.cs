using Application.DTOs.Auth;

namespace Application.Interfaces
{
    public interface IUserService
    {
        Task<AuthResponse> RegisterAsync(string username, string email, string password, string role);
        Task<AuthResponse> LoginAsync(string email, string password);
        Task<AuthResponse> RefreshTokenAsync(string refreshToken);
    }
}
