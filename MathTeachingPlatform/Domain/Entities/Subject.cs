using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    [Table("subjects")]
    public class Subject
    {
        [Key]
        [Column("subject_id")]
        public int SubjectId { get; set; }

        [Required]
        [Column("teacher_id")]
        public int TeacherId { get; set; }

        [Required]
        [MaxLength(255)]
        [Column("title")]
        public string Title { get; set; } = string.Empty;

        [Column("description")]
        public string? Description { get; set; }

        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Column("updated_at")]
        public DateTime? UpdatedAt { get; set; }


        public virtual ICollection<Class> Classes { get; set; } = new List<Class>();
        public virtual ICollection<Syllabus> Syllabi { get; set; } = new List<Syllabus>();
        public virtual ICollection<ExamMatrix> ExamMatrices { get; set; } = new List<ExamMatrix>();
    }
}
