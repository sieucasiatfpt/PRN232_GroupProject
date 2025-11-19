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

        public async Task<string> RegisterAsync(string username, string email, string password, string role)
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

            return _jwtService.GenerateAccessToken(user);
        }

        public async Task<string> LoginAsync(string email, string password)
        {
            var user = await _authUow.Users.FirstOrDefaultAsync(x => x.Email == email);

            if (user == null)
                throw new Exception("Invalid email or password");

            if (!BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
                throw new Exception("Invalid email or password");

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

            return _jwtService.GenerateAccessToken(user);
        }
    }
}
