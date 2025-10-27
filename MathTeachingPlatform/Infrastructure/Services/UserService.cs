using Application.Interfaces;
using Domain.Entities;
using Domain.Enum;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System.Text;
using BCrypt.Net; 

namespace Infrastructure.Services
{
    public class UserService : IUserService
    {
        private readonly AuthDbContext _db;
        public UserService(AuthDbContext db) { _db = db; }

        public async Task<string> RegisterAsync(string username, string password, string role)
        {
            if (await _db.Users.AnyAsync(u => u.Username == username))
                throw new Exception("Username already taken");

            var user = new User
            {
                Username = username,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(password), 
                Role = Enum.Parse<UserRole>(role, true),
                CreatedAt = DateTime.UtcNow
            };

            _db.Users.Add(user);
            await _db.SaveChangesAsync();

            return $"registered:{user.UserId}";
        }

        public async Task<string> LoginAsync(string username, string password)
        {
            var user = await _db.Users.FirstOrDefaultAsync(x => x.Username == username);

            if (user == null || !BCrypt.Net.BCrypt.Verify(password, user.PasswordHash)) 
                throw new Exception("Invalid credentials");

            return $"ok:{user.UserId}";
        }
    }
}
