using Application.DTOs.Payment;
using Application.Interfaces;
using Application.Interfaces.Repositories;
using Application.Models.Payment;
using Domain.Entities;
using Domain.Enum;

namespace Infrastructure.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IPaymentRepository _paymentRepository;
        private readonly IMomoService _momoService;

        public PaymentService(IPaymentRepository paymentRepository, IMomoService momoService)
        {
            _paymentRepository = paymentRepository;
            _momoService = momoService;
        }

        public async Task<PaymentWithMomoDto> CreatePaymentWithMomoAsync(CreatePaymentDto createPaymentDto)
        {
            var payment = new Payment
            {
                TeacherId = createPaymentDto.TeacherId,
                Amount = createPaymentDto.Amount,
                Method = PaymentMethod.Momo,
                Status = PaymentStatus.Pending,
                PaymentDate = DateTime.UtcNow
            };

    
            var savedPayment = await _paymentRepository.CreateAsync(payment);

     
            var orderInfo = new OrderInfoModel
            {
                FullName = createPaymentDto.TeacherName,
                Amount = createPaymentDto.Amount,
                OrderInfo = createPaymentDto.Description ?? $"Payment for Teacher ID: {createPaymentDto.TeacherId}",
                ExtraData = savedPayment.PaymentId.ToString() 
            };

            var momoResponse = await _momoService.CreatePaymentAsync(orderInfo);

   
            return new PaymentWithMomoDto
            {
                PaymentId = savedPayment.PaymentId,
                TeacherId = savedPayment.TeacherId,
                TeacherName = createPaymentDto.TeacherName,
                Amount = savedPayment.Amount,
                PaymentDate = savedPayment.PaymentDate,
                Method = savedPayment.Method,
                Status = savedPayment.Status,
                Description = createPaymentDto.Description,
                MomoPaymentUrl = momoResponse.PayUrl ?? string.Empty,
                MomoOrderId = momoResponse.OrderId ?? string.Empty
            };
        }


        public async Task<PaymentResponseDto?> GetPaymentByIdAsync(int paymentId)
        {
            var payment = await _paymentRepository.GetByIdAsync(paymentId);
            if (payment == null) return null;

            return MapToResponseDto(payment);
        }

        public async Task<IEnumerable<PaymentResponseDto>> GetAllPaymentsAsync()
        {
            var payments = await _paymentRepository.GetAllAsync();
            return payments.Select(MapToResponseDto);
        }

        public async Task<IEnumerable<PaymentResponseDto>> GetPaymentsByTeacherIdAsync(int teacherId)
        {
            var payments = await _paymentRepository.GetByTeacherIdAsync(teacherId);
            return payments.Select(MapToResponseDto);
        }

        public async Task<IEnumerable<PaymentResponseDto>> GetPaymentsByStatusAsync(PaymentStatus status)
        {
            var payments = await _paymentRepository.GetByStatusAsync(status);
            return payments.Select(MapToResponseDto);
        }

        public async Task<PaymentResponseDto?> UpdatePaymentAsync(UpdatePaymentDto updatePaymentDto)
        {
            var existingPayment = await _paymentRepository.GetByIdAsync(updatePaymentDto.PaymentId);
            if (existingPayment == null) return null;

            // Update only provided fields
            if (updatePaymentDto.Amount.HasValue)
                existingPayment.Amount = updatePaymentDto.Amount.Value;

            if (updatePaymentDto.Method.HasValue)
                existingPayment.Method = updatePaymentDto.Method.Value;

            if (updatePaymentDto.Status.HasValue)
                existingPayment.Status = updatePaymentDto.Status.Value;


            var updatedPayment = await _paymentRepository.UpdateAsync(existingPayment);
            return MapToResponseDto(updatedPayment);
        }

        public async Task<bool> DeletePaymentAsync(int paymentId)
        {
            return await _paymentRepository.DeleteAsync(paymentId);
        }

        private static PaymentResponseDto MapToResponseDto(Payment payment)
        {
            return new PaymentResponseDto
            {
                PaymentId = payment.PaymentId,
                TeacherId = payment.TeacherId,
                TeacherName = payment.Teacher?.Name ?? string.Empty,
                Amount = payment.Amount,
                PaymentDate = payment.PaymentDate,
                Method = payment.Method,
                Status = payment.Status,
            };
        }
    }
}