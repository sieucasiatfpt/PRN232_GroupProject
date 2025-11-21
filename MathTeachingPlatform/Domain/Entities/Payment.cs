using Domain.Enum;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    [Table("payments")]
    public class Payment
    {
        [Key]
        [Column("payment_id")]
        public int PaymentId { get; set; }

        [Required]
        [Column("teacher_id")]
        public int TeacherId { get; set; }

        [Required]
        [Column("amount", TypeName = "decimal(10,2)")]
        public decimal Amount { get; set; }

        [Column("payment_date")]
        public DateTime PaymentDate { get; set; } = DateTime.UtcNow;

        [Required]
        [Column("payment_method")]
        public PaymentMethod Method { get; set; }

        [Required]
        [Column("status")]
        public PaymentStatus Status { get; set; } = PaymentStatus.Pending;

        // Navigation Properties
        [ForeignKey("TeacherId")]
        public virtual Teacher Teacher { get; set; } = null!;
    }

}
