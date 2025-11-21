using Application.DTOs.Payment;
using Domain.Enum;

namespace Application.Interfaces
{
    public interface IPaymentService
    {
        Task<PaymentWithMomoDto> CreatePaymentWithMomoAsync(CreatePaymentDto createPaymentDto);
        Task<PaymentResponseDto?> GetPaymentByIdAsync(int paymentId);
        Task<IEnumerable<PaymentResponseDto>> GetAllPaymentsAsync();
        Task<IEnumerable<PaymentResponseDto>> GetPaymentsByTeacherIdAsync(int teacherId);
        Task<IEnumerable<PaymentResponseDto>> GetPaymentsByStatusAsync(PaymentStatus status);
        Task<PaymentResponseDto?> UpdatePaymentAsync(UpdatePaymentDto updatePaymentDto);
        Task<bool> DeletePaymentAsync(int paymentId);
    }
}