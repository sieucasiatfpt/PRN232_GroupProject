using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence
{
    public class AiDbContext : DbContext
    {
        public AiDbContext(DbContextOptions<AiDbContext> opts) : base(opts) { }

        public DbSet<AIConfig> AIConfigs => Set<AIConfig>();
        public DbSet<AICallLog> AICallLogs => Set<AICallLog>();
        public DbSet<AIHistoryChat> AIHistoryChats => Set<AIHistoryChat>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AIConfig>(b =>
            {
                b.ToTable("ai_configs");
                b.HasKey(x => x.ConfigId);

                b.Property(x => x.ConfigId).HasColumnName("config_id");
                b.Property(x => x.TeacherId).HasColumnName("teacher_id").IsRequired();
                b.Property(x => x.ConfigName).HasColumnName("config_name").HasMaxLength(100).IsRequired();
                b.Property(x => x.ModelName).HasColumnName("model_name").HasMaxLength(100);
                b.Property(x => x.Temperature).HasColumnName("temperature").HasColumnType("decimal(3,2)");
                b.Property(x => x.MaxTokens).HasColumnName("max_tokens");
                b.Property(x => x.ConfigDetails).HasColumnName("config_details").HasColumnType("nvarchar(max)");
                b.Property(x => x.SettingsJson).HasColumnName("settings_json").HasColumnType("nvarchar(max)");
                b.Property(x => x.IsActive).HasColumnName("is_active").IsRequired();
                b.Property(x => x.CreatedAt).HasColumnName("created_at").IsRequired();

                b.HasIndex(x => x.TeacherId);
                b.HasIndex(x => x.IsActive);
                b.HasIndex(x => new { x.TeacherId, x.IsActive });

                b.HasMany(x => x.AICallLogs)
                    .WithOne(cl => cl.AIConfig)
                    .HasForeignKey(cl => cl.ConfigId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<AICallLog>(b =>
            {
                b.ToTable("ai_call_logs");
                b.HasKey(x => x.LogId);

                b.Property(x => x.LogId).HasColumnName("log_id");
                b.Property(x => x.ConfigId).HasColumnName("config_id").IsRequired();
                b.Property(x => x.StudentId).HasColumnName("student_id");
                b.Property(x => x.MatrixId).HasColumnName("matrix_id");
                b.Property(x => x.ServiceName).HasColumnName("service_name").HasMaxLength(100);
                b.Property(x => x.RequestText).HasColumnName("request_text").HasColumnType("nvarchar(max)");
                b.Property(x => x.RequestData).HasColumnName("request_data").HasColumnType("nvarchar(max)");
                b.Property(x => x.ResponseText).HasColumnName("response_text").HasColumnType("nvarchar(max)");
                b.Property(x => x.ResponseData).HasColumnName("response_data").HasColumnType("nvarchar(max)");
                b.Property(x => x.CreatedAt).HasColumnName("created_at").IsRequired();

                b.HasIndex(x => x.ConfigId);
                b.HasIndex(x => x.StudentId);
                b.HasIndex(x => x.MatrixId);
                b.HasIndex(x => x.CreatedAt);
            });

            modelBuilder.Entity<AIHistoryChat>(b =>
            {
                b.ToTable("ai_history_chats");
                b.HasKey(x => x.ChatId);

                b.Property(x => x.ChatId).HasColumnName("chat_id");
                b.Property(x => x.TeacherId).HasColumnName("teacher_id").IsRequired();
                b.Property(x => x.Message).HasColumnName("message").HasColumnType("nvarchar(max)").IsRequired();
                b.Property(x => x.ChatSummary).HasColumnName("chat_summary").HasColumnType("nvarchar(max)");
                b.Property(x => x.CreatedAt).HasColumnName("created_at").IsRequired();

                b.HasIndex(x => x.TeacherId);
                b.HasIndex(x => x.CreatedAt);
            });

            modelBuilder.Entity<ClassStudent>()
        .HasKey(cs => new { cs.ClassId, cs.StudentId });

            base.OnModelCreating(modelBuilder);
        }
    }
}
