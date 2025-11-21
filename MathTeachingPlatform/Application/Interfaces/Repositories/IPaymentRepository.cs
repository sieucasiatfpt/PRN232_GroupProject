using Domain.Entities;
using Domain.Enum;

namespace Application.Interfaces.Repositories
{
    public interface IPaymentRepository
    {
        Task<Payment> CreateAsync(Payment payment);
        Task<Payment?> GetByIdAsync(int paymentId);
        Task<IEnumerable<Payment>> GetAllAsync();
        Task<IEnumerable<Payment>> GetByTeacherIdAsync(int teacherId);
        Task<IEnumerable<Payment>> GetByStatusAsync(PaymentStatus status);
        Task<Payment> UpdateAsync(Payment payment);
        Task<bool> DeleteAsync(int paymentId);
        Task<bool> ExistsAsync(int paymentId);
    }
}