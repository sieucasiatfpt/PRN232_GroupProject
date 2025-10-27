using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence
{
    public class AuthDbContext : DbContext
    {
        public AuthDbContext(DbContextOptions<AuthDbContext> opts) : base(opts) { }

        public DbSet<User> Users => Set<User>();
        public DbSet<Teacher> Teachers => Set<Teacher>();
        public DbSet<Student> Students => Set<Student>();
        public DbSet<Payment> Payments => Set<Payment>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(b =>
            {
                b.ToTable("users");
                b.HasKey(x => x.UserId);

                b.Property(x => x.UserId).HasColumnName("user_id");
                b.Property(x => x.Username).HasColumnName("username").HasMaxLength(100).IsRequired();
                b.Property(x => x.PasswordHash).HasColumnName("password_hash").HasMaxLength(255).IsRequired();
                b.Property(x => x.Email).HasColumnName("email").HasMaxLength(255);
                b.Property(x => x.Role).HasColumnName("role").HasConversion<string>().HasMaxLength(50).IsRequired();
                b.Property(x => x.CreatedAt).HasColumnName("created_at").IsRequired();
                b.Property(x => x.UpdatedAt).HasColumnName("updated_at");

                b.HasIndex(x => x.Username).IsUnique();
                b.HasIndex(x => x.Email);

                b.HasOne(x => x.Teacher)
                    .WithOne(t => t.User)
                    .HasForeignKey<Teacher>(t => t.UserId)
                    .OnDelete(DeleteBehavior.Cascade);

                b.HasOne(x => x.Student)
                    .WithOne(s => s.User)
                    .HasForeignKey<Student>(s => s.UserId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Teacher>(b =>
            {
                b.ToTable("teachers");
                b.HasKey(x => x.TeacherId);

                b.Property(x => x.TeacherId).HasColumnName("teacher_id");
                b.Property(x => x.UserId).HasColumnName("user_id").IsRequired();
                b.Property(x => x.Name).HasColumnName("name").HasMaxLength(255);
                b.Property(x => x.Bio).HasColumnName("bio").HasColumnType("nvarchar(max)");
                b.Property(x => x.HireDate).HasColumnName("hire_date");
                b.Property(x => x.Department).HasColumnName("department").HasMaxLength(100);
                b.Property(x => x.Status).HasColumnName("status").HasConversion<string>().HasMaxLength(20).IsRequired();

                b.HasIndex(x => x.UserId).IsUnique();
                b.HasIndex(x => x.Status);

                b.HasMany(x => x.Payments)
                    .WithOne(p => p.Teacher)
                    .HasForeignKey(p => p.TeacherId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<Student>(b =>
            {
                b.ToTable("students");
                b.HasKey(x => x.StudentId);

                b.Property(x => x.StudentId).HasColumnName("student_id");
                b.Property(x => x.UserId).HasColumnName("user_id").IsRequired();
                b.Property(x => x.Name).HasColumnName("name").HasMaxLength(255);
                b.Property(x => x.EnrollmentDate).HasColumnName("enrollment_date");
                b.Property(x => x.Major).HasColumnName("major").HasMaxLength(100);
                b.Property(x => x.Status).HasColumnName("status").HasConversion<string>().HasMaxLength(20).IsRequired();

                b.HasIndex(x => x.UserId).IsUnique();
                b.HasIndex(x => x.Status);
            });

            modelBuilder.Entity<Payment>(b =>
            {
                b.ToTable("payments");
                b.HasKey(x => x.PaymentId);

                b.Property(x => x.PaymentId).HasColumnName("payment_id");
                b.Property(x => x.TeacherId).HasColumnName("teacher_id").IsRequired();
                b.Property(x => x.Amount).HasColumnName("amount").HasColumnType("decimal(10,2)").IsRequired();
                b.Property(x => x.PaymentDate).HasColumnName("payment_date").IsRequired();
                b.Property(x => x.Method).HasColumnName("payment_method").HasConversion<string>().HasMaxLength(20).IsRequired();
                b.Property(x => x.Status).HasColumnName("status").HasConversion<string>().HasMaxLength(20).IsRequired();

                b.HasIndex(x => x.TeacherId);
                b.HasIndex(x => x.Status);
                b.HasIndex(x => x.PaymentDate);
            });

            modelBuilder.Entity<ClassStudent>()
        .HasKey(cs => new { cs.ClassId, cs.StudentId });

            base.OnModelCreating(modelBuilder);
        }
    }
}
