using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

namespace Infrastructure.Persistence
{
    public class ContentDbContext : DbContext
    {
        public ContentDbContext(DbContextOptions<ContentDbContext> opts) : base(opts) { }

        public DbSet<Class> Classes => Set<Class>();
        public DbSet<ClassStudent> ClassStudents => Set<ClassStudent>();
        public DbSet<Subject> Subjects => Set<Subject>();
        public DbSet<Syllabus> Syllabi => Set<Syllabus>();
        public DbSet<ExamMatrix> ExamMatrices => Set<ExamMatrix>();
        public DbSet<ExamQuestion> ExamQuestions => Set<ExamQuestion>();
        public DbSet<Activity> Activities => Set<Activity>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Subject>(b =>
            {
                b.ToTable("subjects");
                b.HasKey(x => x.SubjectId);

                b.Property(x => x.SubjectId).HasColumnName("subject_id");
                b.Property(x => x.TeacherId).HasColumnName("teacher_id").IsRequired();
                b.Property(x => x.Title).HasColumnName("title").HasMaxLength(255).IsRequired();
                b.Property(x => x.Description).HasColumnName("description").HasColumnType("nvarchar(max)");
                b.Property(x => x.CreatedAt).HasColumnName("created_at").IsRequired();
                b.Property(x => x.UpdatedAt).HasColumnName("updated_at");

                b.HasIndex(x => x.TeacherId);
                b.HasIndex(x => x.Title);

                b.HasMany(x => x.Classes)
                    .WithOne(c => c.Subject)
                    .HasForeignKey(c => c.SubjectId)
                    .OnDelete(DeleteBehavior.Restrict);

                b.HasMany(x => x.Syllabi)
                    .WithOne(s => s.Subject)
                    .HasForeignKey(s => s.SubjectId)
                    .OnDelete(DeleteBehavior.Restrict);

                b.HasMany(x => x.ExamMatrices)
                    .WithOne(em => em.Subject)
                    .HasForeignKey(em => em.SubjectId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<Class>(b =>
            {
                b.ToTable("classes");
                b.HasKey(x => x.ClassId);

                b.Property(x => x.ClassId).HasColumnName("class_id");
                b.Property(x => x.SubjectId).HasColumnName("subject_id").IsRequired();
                b.Property(x => x.TeacherId).HasColumnName("teacher_id").IsRequired();
                b.Property(x => x.Name).HasColumnName("name").HasMaxLength(255).IsRequired();
                b.Property(x => x.Schedule).HasColumnName("schedule").HasMaxLength(500);
                b.Property(x => x.StartDate).HasColumnName("start_date");
                b.Property(x => x.EndDate).HasColumnName("end_date");
                b.Property(x => x.CreatedAt).HasColumnName("created_at").IsRequired();

                b.HasIndex(x => x.SubjectId);
                b.HasIndex(x => x.TeacherId);
                b.HasIndex(x => x.StartDate);

                b.HasMany(x => x.ClassStudents)
                    .WithOne(cs => cs.Class)
                    .HasForeignKey(cs => cs.ClassId)
                    .OnDelete(DeleteBehavior.Cascade);

                b.HasMany(x => x.Activities)
                    .WithOne(a => a.Class)
                    .HasForeignKey(a => a.ClassId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<ClassStudent>(b =>
            {
                b.ToTable("class_students");
                b.HasKey(x => new { x.ClassId, x.StudentId });

                b.Property(x => x.ClassId).HasColumnName("class_id");
                b.Property(x => x.StudentId).HasColumnName("student_id");
                b.Property(x => x.EnrollmentStatus).HasColumnName("enrollment_status").HasMaxLength(50).IsRequired();
                b.Property(x => x.EnrolledAt).HasColumnName("enrolled_at").IsRequired();

                b.HasIndex(x => x.StudentId);
                b.HasIndex(x => x.EnrollmentStatus);
            });

            modelBuilder.Entity<Syllabus>(b =>
            {
                b.ToTable("syllabi");
                b.HasKey(x => x.SyllabusId);

                b.Property(x => x.SyllabusId).HasColumnName("syllabus_id");
                b.Property(x => x.TeacherId).HasColumnName("teacher_id").IsRequired();
                b.Property(x => x.SubjectId).HasColumnName("subject_id").IsRequired();
                b.Property(x => x.Title).HasColumnName("title").HasMaxLength(255).IsRequired();
                b.Property(x => x.Description).HasColumnName("description").HasColumnType("nvarchar(max)");
                b.Property(x => x.Url).HasColumnName("url").HasMaxLength(500);
                b.Property(x => x.CreatedAt).HasColumnName("created_at").IsRequired();

                b.HasIndex(x => x.TeacherId);
                b.HasIndex(x => x.SubjectId);

                b.HasMany(x => x.ExamQuestions)
                    .WithOne(eq => eq.Syllabus)
                    .HasForeignKey(eq => eq.SyllabusId)
                    .OnDelete(DeleteBehavior.SetNull);
            });

            modelBuilder.Entity<ExamMatrix>(b =>
            {
                b.ToTable("exam_matrices");
                b.HasKey(x => x.MatrixId);

                b.Property(x => x.MatrixId).HasColumnName("matrix_id");
                b.Property(x => x.SubjectId).HasColumnName("subject_id").IsRequired();
                b.Property(x => x.Title).HasColumnName("title").HasMaxLength(255).IsRequired();
                b.Property(x => x.DifficultyDistribution).HasColumnName("difficulty_distribution").HasColumnType("nvarchar(max)");
                b.Property(x => x.TotalQuestions).HasColumnName("total_questions").IsRequired();
                b.Property(x => x.GeneratedOn).HasColumnName("generated_on").IsRequired();

                b.HasIndex(x => x.SubjectId);
                b.HasIndex(x => x.GeneratedOn);

                b.HasMany(x => x.ExamQuestions)
                    .WithOne(eq => eq.ExamMatrix)
                    .HasForeignKey(eq => eq.MatrixId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<ExamQuestion>(b =>
            {
                b.ToTable("exam_questions");
                b.HasKey(x => x.QuestionId);

                b.Property(x => x.QuestionId).HasColumnName("question_id");
                b.Property(x => x.SyllabusId).HasColumnName("syllabus_id");
                b.Property(x => x.MatrixId).HasColumnName("matrix_id");
                b.Property(x => x.QuestionText).HasColumnName("question_text").HasColumnType("nvarchar(max)").IsRequired();
                b.Property(x => x.QuestionType).HasColumnName("question_type").HasConversion<string>().HasMaxLength(30).IsRequired();
                b.Property(x => x.OptionsJson).HasColumnName("options_json").HasColumnType("nvarchar(max)");
                b.Property(x => x.Answers).HasColumnName("answers").HasColumnType("nvarchar(max)").IsRequired();
                b.Property(x => x.Marks).HasColumnName("marks");
                b.Property(x => x.Points).HasColumnName("points").HasColumnType("decimal(5,2)");

                b.HasIndex(x => x.SyllabusId);
                b.HasIndex(x => x.MatrixId);
                b.HasIndex(x => x.QuestionType);
            });

            modelBuilder.Entity<Activity>(b =>
            {
                b.ToTable("activities");
                b.HasKey(x => x.ActivityId);

                b.Property(x => x.ActivityId).HasColumnName("activity_id");
                b.Property(x => x.ClassId).HasColumnName("class_id").IsRequired();
                b.Property(x => x.ExamAttemptId).HasColumnName("exam_attempt_id");
                b.Property(x => x.Title).HasColumnName("title").HasMaxLength(255).IsRequired();
                b.Property(x => x.ActivityType).HasColumnName("activity_type").HasMaxLength(50).IsRequired();
                b.Property(x => x.Description).HasColumnName("description").HasColumnType("nvarchar(max)");
                b.Property(x => x.DueDate).HasColumnName("due_date");
                b.Property(x => x.CreatedAt).HasColumnName("created_at").IsRequired();

                b.HasIndex(x => x.ClassId);
                b.HasIndex(x => x.ExamAttemptId);
                b.HasIndex(x => x.ActivityType);
                b.HasIndex(x => x.DueDate);
            });

            base.OnModelCreating(modelBuilder);
        }
    }
}
