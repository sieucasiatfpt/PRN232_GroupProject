using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence
{
    public class ExamDbContext : DbContext
    {
        public ExamDbContext(DbContextOptions<ExamDbContext> opts) : base(opts) { }

        public DbSet<ExamAttempt> ExamAttempts => Set<ExamAttempt>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ExamAttempt>(b =>
            {
                b.ToTable("exam_attempts");
                b.HasKey(x => x.AttemptId);

                b.Property(x => x.AttemptId).HasColumnName("attempt_id");
                b.Property(x => x.StudentId).HasColumnName("student_id").IsRequired();
                b.Property(x => x.StartTime).HasColumnName("start_time");
                b.Property(x => x.EndTime).HasColumnName("end_time");
                b.Property(x => x.Score).HasColumnName("score").HasColumnType("decimal(5,2)");
                b.Property(x => x.AttemptNumber).HasColumnName("attempt_number").IsRequired();
                b.Property(x => x.Status).HasColumnName("status").HasConversion<string>().HasMaxLength(30).IsRequired();
                b.Property(x => x.CreatedAt).HasColumnName("created_at").IsRequired();

                b.HasIndex(x => x.StudentId);
                b.HasIndex(x => x.Status);
                b.HasIndex(x => new { x.StudentId, x.AttemptNumber });
                b.HasIndex(x => x.StartTime);
            });

            modelBuilder.Entity<ClassStudent>()
        .HasKey(cs => new { cs.ClassId, cs.StudentId });

            base.OnModelCreating(modelBuilder);
        }
    }
}
