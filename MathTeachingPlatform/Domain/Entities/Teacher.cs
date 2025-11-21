using Domain.Enum;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    [Table("teachers")]
    public class Teacher
    {
        [Key]
        [Column("teacher_id")]
        public int TeacherId { get; set; }

        [Required]
        [Column("user_id")]
        public int UserId { get; set; }

        [MaxLength(255)]
        [Column("name")]
        public string? Name { get; set; }

        [Column("bio")]
        public string? Bio { get; set; }

        [Column("hire_date")]
        public DateTime? HireDate { get; set; }

        [MaxLength(100)]
        [Column("department")]
        public string? Department { get; set; }

        [Required]
        [Column("status")]
        public TeacherStatus Status { get; set; } = TeacherStatus.Active;

        // Navigation Properties
        [ForeignKey("UserId")]
        public virtual User User { get; set; } = null!;

        public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();
    }
}
