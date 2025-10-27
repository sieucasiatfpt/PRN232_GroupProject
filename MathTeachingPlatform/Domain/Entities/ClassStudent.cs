using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    [Table("class_students")]
    public class ClassStudent
    {
        [Column("class_id")]
        public int ClassId { get; set; }

        [Column("student_id")]
        public int StudentId { get; set; }

        [Required]
        [MaxLength(50)]
        [Column("enrollment_status")]
        public string EnrollmentStatus { get; set; } = "Active";

        [Column("enrolled_at")]
        public DateTime EnrolledAt { get; set; } = DateTime.UtcNow;

        // Navigation Properties
        [ForeignKey("ClassId")]
        public virtual Class Class { get; set; } = null!;

        [ForeignKey("StudentId")]
        public virtual Student Student { get; set; } = null!;
    }
}
