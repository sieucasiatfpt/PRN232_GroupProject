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

        public UserService(IAuthUnitOfWork authUow)
        {
            _authUow = authUow;
        }

        public async Task<string> RegisterAsync(string username, string password, string role)
        {
            // Check if username already exists
            if (await _authUow.Users.AnyAsync(u => u.Username == username))
                throw new Exception("Username already taken");

            var user = new User
            {
                Username = username,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(password),
                Role = Enum.Parse<UserRole>(role, true),
                CreatedAt = DateTime.UtcNow
            };

            await _authUow.Users.AddAsync(user);
            await _authUow.SaveChangesAsync();

            return $"registered:{user.UserId}";
        }

        public async Task<string> LoginAsync(string username, string password)
        {
            var user = await _authUow.Users.FirstOrDefaultAsync(x => x.Username == username);

            if (user == null || !BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
                throw new Exception("Invalid credentials");

            return $"ok:{user.UserId}";
        }
    }
}
