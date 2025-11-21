using Domain.Enum;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    [Table("students")]
    public class Student
    {
        [Key]
        [Column("student_id")]
        public int StudentId { get; set; }

        [Required]
        [Column("user_id")]
        public int UserId { get; set; }

        [MaxLength(255)]
        [Column("name")]
        public string? Name { get; set; }

        [Column("enrollment_date")]
        public DateTime? EnrollmentDate { get; set; }

        [MaxLength(100)]
        [Column("major")]
        public string? Major { get; set; }

        [Required]
        [Column("status")]
        public StudentStatus Status { get; set; } = StudentStatus.Active;

        // Navigation Properties
        [ForeignKey("UserId")]
        public virtual User User { get; set; } = null!;

        public virtual ICollection<ClassStudent> ClassStudents { get; set; } = new List<ClassStudent>();
        public virtual ICollection<ExamAttempt> ExamAttempts { get; set; } = new List<ExamAttempt>();
        public virtual ICollection<AICallLog> AICallLogs { get; set; } = new List<AICallLog>();
    }
}
