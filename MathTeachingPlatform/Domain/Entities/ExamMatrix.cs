using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    [Table("exam_matrices")]
    public class ExamMatrix
    {
        [Key]
        [Column("matrix_id")]
        public int MatrixId { get; set; }

        [Required]
        [Column("subject_id")]
        public int SubjectId { get; set; }

        [Required]
        [MaxLength(255)]
        [Column("title")]
        public string Title { get; set; } = string.Empty;

        [Column("difficulty_distribution")]
        public string? DifficultyDistribution { get; set; }

        [Column("total_questions")]
        public int TotalQuestions { get; set; }

        [Column("generated_on")]
        public DateTime GeneratedOn { get; set; } = DateTime.UtcNow;

        // Navigation Properties
        [ForeignKey("SubjectId")]
        public virtual Subject Subject { get; set; } = null!;

        public virtual ICollection<ExamQuestion> ExamQuestions { get; set; } = new List<ExamQuestion>();
        public virtual ICollection<AICallLog> AICallLogs { get; set; } = new List<AICallLog>();
    }
}
