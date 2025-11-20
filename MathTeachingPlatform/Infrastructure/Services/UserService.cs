using Application.DTOs.Auth;
using Application.Interfaces;
using Application.Interfaces.Repositories;
using BCrypt.Net; 
using Domain.Entities;
using Domain.Enum;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System.Text;

namespace Infrastructure.Services
{
    public class UserService : IUserService
    {
        private readonly IAuthUnitOfWork _authUow;
        private readonly IContentUnitOfWork _contentUow;
        private readonly IJwtService _jwtService;

        public UserService(IAuthUnitOfWork authUow, IContentUnitOfWork contentUow, IJwtService jwtService)
        {
            _authUow = authUow;
            _contentUow = contentUow;
            _jwtService = jwtService;
        }

        public async Task<AuthResponse> RegisterAsync(string username, string email, string password, string role)
        {
            if (await _authUow.Users.AnyAsync(u => u.Username == username))
                throw new Exception("Username already taken");

            if (await _authUow.Users.AnyAsync(u => u.Email == email))
                throw new Exception("Email already registered");

            if (!Enum.TryParse<UserRole>(role, true, out var userRole))
                throw new Exception("Invalid role");

            var user = new User
            {
                Username = username,
                Email = email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(password),
                Role = userRole,
                CreatedAt = DateTime.UtcNow
            };

            await _authUow.Users.AddAsync(user);
            await _authUow.SaveChangesAsync();

            var accessToken = _jwtService.GenerateAccessToken(user);

            return new AuthResponse
            {
                AccessToken = accessToken,
                AccessTokenExpiry = DateTime.UtcNow.AddMinutes(_jwtService.GetAccessTokenExpirationMinutes()),
                Email = email,
                Role = userRole.ToString(),
                Message = "Registration successful"
            };
        }

        public async Task<AuthResponse> LoginAsync(string email, string password)
        {
            var user = await _authUow.Users.FirstOrDefaultAsync(x => x.Email == email);

            if (user == null)
                throw new Exception("Invalid email or password");

            if (!BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
                throw new Exception("Invalid email or password");

            // Check user status based on role
            if (user.Role == UserRole.Student)
            {
                var student = await _authUow.Students
                    .FirstOrDefaultAsync(s => s.UserId == user.UserId);

                if (student != null && student.Status == StudentStatus.Suspended)
                {
                    throw new Exception("Your student account has been suspended. Please contact the administrator.");
                }
            }
            else if (user.Role == UserRole.Teacher)
            {
                var teacher = await _authUow.Teachers
                    .FirstOrDefaultAsync(t => t.UserId == user.UserId);

                if (teacher != null && teacher.Status == TeacherStatus.Inactive)
                {
                    throw new Exception("Your teacher account is inactive. Please contact the administrator.");
                }
            }

            // Generate access token
            var accessToken = _jwtService.GenerateAccessToken(user);

            return new AuthResponse
            {
                AccessToken = accessToken,
                AccessTokenExpiry = DateTime.UtcNow.AddMinutes(_jwtService.GetAccessTokenExpirationMinutes()),
                Email = email,
                Role = user.Role.ToString(),
                Message = "Login successful"
            };
        }

        public async Task<AuthResponse> RefreshTokenAsync(string refreshToken)
        {
            // Validate the refresh token format
            if (string.IsNullOrWhiteSpace(refreshToken))
                throw new Exception("Invalid refresh token");

            throw new Exception("Refresh token validation should be done at controller level with session");
        }

        public async Task<bool> UpdatePaymentStatusAsync(int userId, PaymentStatus paymentStatus)
        {
            var user = await _authUow.Users.GetByIdAsync(userId);
            if (user == null)
            {
                return false;
            }

            user.PaymentStatus = paymentStatus;
            //user.UpdatedAt = DateTime.UtcNow;

            _authUow.Users.Update(user);
            await _authUow.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            return await _authUow.Users.GetAllAsync();
        }
    }
}
