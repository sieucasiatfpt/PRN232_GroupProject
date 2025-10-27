using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    [Table("activities")]
    public class Activity
    {
        [Key]
        [Column("activity_id")]
        public int ActivityId { get; set; }

        [Required]
        [Column("class_id")]
        public int ClassId { get; set; }

        [Column("exam_attempt_id")]
        public int? ExamAttemptId { get; set; }

        [Required]
        [MaxLength(255)]
        [Column("title")]
        public string Title { get; set; } = string.Empty;

        [Required]
        [MaxLength(50)]
        [Column("activity_type")]
        public string ActivityType { get; set; } = string.Empty; // Assignment, Quiz, Exam

        [Column("description")]
        public string? Description { get; set; }

        [Column("due_date")]
        public DateTime? DueDate { get; set; }

        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation Properties
        [ForeignKey("ClassId")]
        public virtual Class Class { get; set; } = null!;

        [ForeignKey("ExamAttemptId")]
        public virtual ExamAttempt? ExamAttempt { get; set; }
    }
}
