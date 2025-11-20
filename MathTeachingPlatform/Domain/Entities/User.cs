using Domain.Enum;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    [Table("users")]
    public class User
    {
        [Key]
        [Column("user_id")]
        public int UserId { get; set; }

        [Required]
        [MaxLength(100)]
        [Column("username")]
        public string Username { get; set; } = string.Empty;

        [Required]
        [Column("password_hash")]
        public string PasswordHash { get; set; } = string.Empty;

        [MaxLength(255)]
        [Column("email")]
        public string? Email { get; set; }

        [Required]
        [Column("role")]
        public UserRole Role { get; set; }

        [Required]
        [Column("payment_status")]
        public PaymentStatus PaymentStatus { get; set; } = PaymentStatus.Pending;

        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Column("updated_at")]
        public DateTime? UpdatedAt { get; set; }

        // Navigation Properties
        public virtual Teacher? Teacher { get; set; }
        public virtual Student? Student { get; set; }
    }
}
