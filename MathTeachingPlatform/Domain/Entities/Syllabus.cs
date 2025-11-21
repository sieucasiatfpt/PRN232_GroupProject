using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    [Table("syllabi")]
    public class Syllabus
    {
        [Key]
        [Column("syllabus_id")]
        public int SyllabusId { get; set; }

        [Required]
        [Column("teacher_id")]
        public int TeacherId { get; set; }

        [Required]
        [Column("subject_id")]
        public int SubjectId { get; set; }

        [Required]
        [MaxLength(255)]
        [Column("title")]
        public string Title { get; set; } = string.Empty;

        [Column("description")]
        public string? Description { get; set; }

        [MaxLength(500)]
        [Column("url")]
        public string? Url { get; set; }

        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation Properties

        [ForeignKey("SubjectId")]
        public virtual Subject Subject { get; set; } = null!;

        public virtual ICollection<ExamQuestion> ExamQuestions { get; set; } = new List<ExamQuestion>();
    }
}
