using System;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Enum;
using Infrastructure.Persistence;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Seed
{
    public static class DatabaseSeeder
    {
        public static async Task SeedAsync(IServiceProvider services, ILogger logger)
        {
            await using var scope = services.CreateAsyncScope();
            var provider = scope.ServiceProvider;

            var authDb = provider.GetRequiredService<AuthDbContext>();
            var contentDb = provider.GetRequiredService<ContentDbContext>();
            var examDb = provider.GetRequiredService<ExamDbContext>();
            var aiDb = provider.GetRequiredService<AiDbContext>();

            var cfg = provider.GetRequiredService<IConfiguration>();
            var autoMigrate = cfg.GetValue<bool>("Database:AutoMigrate");

            if (autoMigrate)
            {
                logger.LogInformation("Applying migrations...");
                await authDb.Database.MigrateAsync();
                await contentDb.Database.MigrateAsync();
                await examDb.Database.MigrateAsync();
                await aiDb.Database.MigrateAsync();
                logger.LogInformation("Databases migrated successfully.");
            }
            else
            {
                logger.LogInformation("Ensuring databases created (no migrations)...");
                await authDb.Database.EnsureCreatedAsync();
                await contentDb.Database.EnsureCreatedAsync();
                await examDb.Database.EnsureCreatedAsync();
                await aiDb.Database.EnsureCreatedAsync();
                logger.LogInformation("Databases ensured created.");
            }

            try
            {
                await SeedAdminUserAsync(authDb, logger);
            }
            catch (Exception ex)
            {
                logger.LogWarning("Skipping admin seeding due to database unavailability or missing schema: {Message}", ex.Message);
            }
        }

        private static async Task SeedAdminUserAsync(AuthDbContext db, ILogger logger)
        {
            // check if admin exists
            if (await db.Users.AnyAsync(u => u.Username == "admin")) return;

            logger.LogInformation("Seeding default admin user...");

            db.Users.Add(new User
            {
                Username = "admin",
                Email = "admin@gmail.com",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin@123"),
                Role = UserRole.Admin,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            });

            await db.SaveChangesAsync();

            logger.LogInformation("Admin user seeded (email: admin@gmail.com, password: Admin@123).");
        }
    }
}
