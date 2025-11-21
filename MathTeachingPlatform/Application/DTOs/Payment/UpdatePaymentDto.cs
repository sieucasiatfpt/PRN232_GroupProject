using Domain.Enum;

namespace Application.DTOs.Payment
{
    public class UpdatePaymentDto
    {
        public int PaymentId { get; set; }
        public decimal? Amount { get; set; }
        public PaymentMethod? Method { get; set; }
        public PaymentStatus? Status { get; set; }
    }
}