using Application.Models.Payment;
using Microsoft.AspNetCore.Http;

namespace Application.Interfaces
{
    public interface IMomoService
    {
        Task<MomoCreatePaymentResponseModel> CreatePaymentAsync(OrderInfoModel model);
        MomoExecuteResponseModel PaymentExecuteAsync(IQueryCollection collection);
        Task<MomoCallbackResponseModel> HandlePaymentCallbackAsync(IQueryCollection query);

        Task<bool> ValidateSignature(MomoCallbackResponseModel callbackResponse, string providedSignature);
    }
}