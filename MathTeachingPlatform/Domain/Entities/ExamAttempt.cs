using Domain.Enum;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    [Table("exam_attempts")]
    public class ExamAttempt
    {
        [Key]
        [Column("attempt_id")]
        public int AttemptId { get; set; }

        [Required]
        [Column("student_id")]
        public int StudentId { get; set; }

        [Column("start_time")]
        public DateTime? StartTime { get; set; }

        [Column("end_time")]
        public DateTime? EndTime { get; set; }

        [Column("score", TypeName = "decimal(5,2)")]
        public decimal? Score { get; set; }

        [Column("attempt_number")]
        public int AttemptNumber { get; set; } = 1;

        [Required]
        [Column("status")]
        public ExamAttemptStatus Status { get; set; } = ExamAttemptStatus.InProgress;

        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation Properties
        [ForeignKey("StudentId")]
        public virtual Student Student { get; set; } = null!;

        public virtual ICollection<Activity> Activities { get; set; } = new List<Activity>();
    }
}
