using Domain.Enum;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    [Table("exam_questions")]
    public class ExamQuestion
    {
        [Key]
        [Column("question_id")]
        public int QuestionId { get; set; }

        [Column("syllabus_id")]
        public int? SyllabusId { get; set; }

        [Column("matrix_id")]
        public int? MatrixId { get; set; }

        [Required]
        [Column("question_text")]
        public string QuestionText { get; set; } = string.Empty;

        [Required]
        [Column("question_type")]
        public QuestionType QuestionType { get; set; } = QuestionType.MCQ;

        [Column("options_json")]
        public string? OptionsJson { get; set; }

        [Required]
        [Column("answers")]
        public string Answers { get; set; } = string.Empty;

        [Column("marks")]
        public int? Marks { get; set; }

        [Column("points", TypeName = "decimal(5,2)")]
        public decimal? Points { get; set; }

        // Navigation Properties
        [ForeignKey("SyllabusId")]
        public virtual Syllabus? Syllabus { get; set; }

        [ForeignKey("MatrixId")]
        public virtual ExamMatrix? ExamMatrix { get; set; }
    }
}
