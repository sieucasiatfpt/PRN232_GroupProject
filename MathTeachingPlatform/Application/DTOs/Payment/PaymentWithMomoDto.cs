using Domain.Enum;

namespace Application.DTOs.Payment
{
    public class PaymentWithMomoDto
    {
        public int PaymentId { get; set; }
        public int TeacherId { get; set; }
        public string TeacherName { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public DateTime PaymentDate { get; set; }
        public PaymentMethod Method { get; set; }
        public PaymentStatus Status { get; set; }
        public string? Description { get; set; }
        
        // MOMO Integration
        public string MomoPaymentUrl { get; set; } = string.Empty;
        public string MomoOrderId { get; set; } = string.Empty;
    }
}