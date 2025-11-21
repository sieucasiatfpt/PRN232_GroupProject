using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    [Table("classes")]
    public class Class
    {
        [Key]
        [Column("class_id")]
        public int ClassId { get; set; }

        [Required]
        [Column("subject_id")]
        public int SubjectId { get; set; }

        [Required]
        [Column("teacher_id")]
        public int TeacherId { get; set; }

        [Required]
        [MaxLength(255)]
        [Column("name")]
        public string Name { get; set; } = string.Empty;

        [MaxLength(500)]
        [Column("schedule")]
        public string? Schedule { get; set; }

        [Column("start_date")]
        public DateTime? StartDate { get; set; }

        [Column("end_date")]
        public DateTime? EndDate { get; set; }

        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation Properties
        [ForeignKey("SubjectId")]
        public virtual Subject Subject { get; set; } = null!;

        public virtual ICollection<ClassStudent> ClassStudents { get; set; } = new List<ClassStudent>();
        public virtual ICollection<Activity> Activities { get; set; } = new List<Activity>();
    }
}
