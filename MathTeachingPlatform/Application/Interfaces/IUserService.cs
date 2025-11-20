using Application.DTOs.Auth;
using Domain.Entities;
using Domain.Enum;

namespace Application.Interfaces
{
    public interface IUserService
    {
        Task<AuthResponse> RegisterAsync(string username, string email, string password, string role);
        Task<AuthResponse> LoginAsync(string email, string password);
        Task<AuthResponse> RefreshTokenAsync(string refreshToken);
        Task<bool> UpdatePaymentStatusAsync(int userId, PaymentStatus paymentStatus);
        Task<IEnumerable<User>> GetAllUsersAsync();
    }
}