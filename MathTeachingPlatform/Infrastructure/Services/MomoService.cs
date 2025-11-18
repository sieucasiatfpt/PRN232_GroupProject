using Application.Interfaces;
using Application.Models.Payment;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RestSharp;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace Infrastructure.Services
{
    public class MomoService : IMomoService
    {
        private readonly IOptions<MomoOptionModel> _options;

        public MomoService(IOptions<MomoOptionModel> options)
        {
            _options = options;
        }

        public async Task<MomoCreatePaymentResponseModel> CreatePaymentAsync(OrderInfoModel model)
        {
            // Auto-generate OrderId if not provided
            if (string.IsNullOrEmpty(model.OrderId))
            {
                model.OrderId = DateTime.UtcNow.Ticks.ToString();
            }

            // Gán PaymentId vào ExtraData nếu có
            var extraData = model.ExtraData ?? "";

            model.OrderInfo = "Khách hàng: " + model.FullName + ". Nội dung: " + model.OrderInfo;

            var rawData =
                $"partnerCode={_options.Value.PartnerCode}" +
                $"&accessKey={_options.Value.AccessKey}" +
                $"&requestId={model.OrderId}" +
                $"&amount={model.Amount}" +
                $"&orderId={model.OrderId}" +
                $"&orderInfo={model.OrderInfo}" +
                $"&returnUrl={_options.Value.ReturnUrl}" +
                $"&notifyUrl={_options.Value.NotifyUrl}" +
                $"&extraData={extraData}";

            var signature = ComputeHmacSha256(rawData, _options.Value.SecretKey);

            var client = new RestClient(_options.Value.MomoApiUrl);
            var request = new RestRequest() { Method = Method.Post };
            request.AddHeader("Content-Type", "application/json; charset=UTF-8");

            var requestData = new
            {
                accessKey = _options.Value.AccessKey,
                partnerCode = _options.Value.PartnerCode,
                requestType = _options.Value.RequestType,
                notifyUrl = _options.Value.NotifyUrl,
                returnUrl = _options.Value.ReturnUrl,
                orderId = model.OrderId,
                amount = model.Amount.ToString(),
                orderInfo = model.OrderInfo,
                requestId = model.OrderId,
                extraData = extraData,
                signature = signature
            };

            request.AddParameter("application/json", JsonConvert.SerializeObject(requestData), ParameterType.RequestBody);

            var response = await client.ExecuteAsync(request);
            var momoResponse = JsonConvert.DeserializeObject<MomoCreatePaymentResponseModel>(response.Content);
            return momoResponse;
        }


        public MomoExecuteResponseModel PaymentExecuteAsync(IQueryCollection collection)
        {
            var amount = collection.First(s => s.Key == "amount").Value;
            var orderInfo = collection.First(s => s.Key == "orderInfo").Value;
            var orderId = collection.First(s => s.Key == "orderId").Value;

            return new MomoExecuteResponseModel()
            {
                Amount = amount,
                OrderId = orderId,
                OrderInfo = orderInfo
            };
        }


        public async Task<MomoCallbackResponseModel> HandlePaymentCallbackAsync(IQueryCollection query)
        {
            // Extract parameters from the query collection
            var partnerCode = query["partnerCode"];
            var accessKey = query["accessKey"];
            var requestId = query["requestId"];
            var amount = query["amount"];
            var orderId = query["orderId"];
            var orderInfo = query["orderInfo"];
            var orderType = query["orderType"];
            var transId = query["transId"];
            var message = query["message"];
            var localMessage = query["localMessage"];
            var responseTime = query["responseTime"];
            var errorCode = query["errorCode"];
            var payType = query["payType"];
            var extraData = query["extraData"];
            var signature = query["signature"];

            // Recreate the raw data string for signature validation
            var rawData =
       $"partnerCode={partnerCode}" +
       $"&accessKey={accessKey}" +
       $"&requestId={requestId}" +
       $"&amount={amount}" +
       $"&orderId={orderId}" +
       $"&orderInfo={HttpUtility.UrlDecode(orderInfo)}" +
       $"&orderType={orderType}" +
       $"&transId={transId}" +
       $"&message={HttpUtility.UrlDecode(message)}" +
       $"&localMessage={HttpUtility.UrlDecode(localMessage)}" +
       $"&responseTime={responseTime}" +
       $"&errorCode={errorCode}" +
       $"&payType={payType}" +
       $"&extraData={extraData}";



            // Validate the signature
            var computedSignature = ComputeHmacSha256(rawData, _options.Value.SecretKey);
            if (computedSignature != signature)
            {
                throw new InvalidOperationException("Invalid signature. The callback data may have been tampered with.");
            }

            // Process the payment result
            var callbackResponse = new MomoCallbackResponseModel
            {
                PartnerCode = partnerCode,
                AccessKey = accessKey,
                RequestId = requestId,
                Amount = amount,
                OrderId = orderId,
                OrderInfo = orderInfo,
                OrderType = orderType,
                TransId = transId,
                Message = message,
                LocalMessage = localMessage,
                ResponseTime = responseTime,
                ErrorCode = int.Parse(errorCode),
                PayType = payType,
                ExtraData = extraData
            };

        

            return callbackResponse;
        }


        public async Task<bool> ValidateSignature(MomoCallbackResponseModel callbackResponse, string providedSignature)
        {
            callbackResponse.ExtraData ??= "";

            // Decode fields trước khi tạo rawData
            var orderInfo = HttpUtility.UrlDecode(callbackResponse.OrderInfo);
            var message = HttpUtility.UrlDecode(callbackResponse.Message);
            var localMessage = HttpUtility.UrlDecode(callbackResponse.LocalMessage);

            // Tạo rawData theo thứ tự chuẩn của MoMo
            var rawData =
                $"partnerCode={callbackResponse.PartnerCode}" +
                $"&accessKey={callbackResponse.AccessKey}" +
                $"&requestId={callbackResponse.RequestId}" +
                $"&amount={callbackResponse.Amount}" +
                $"&orderId={callbackResponse.OrderId}" +
                $"&orderInfo={orderInfo}" +
                $"&orderType={callbackResponse.OrderType}" +
                $"&transId={callbackResponse.TransId}" +
                $"&message={message}" +
                $"&localMessage={localMessage}" +
                $"&responseTime={callbackResponse.ResponseTime}" +
                $"&errorCode={callbackResponse.ErrorCode}" +
                $"&payType={callbackResponse.PayType}" +
                $"&extraData={callbackResponse.ExtraData}";

           // Console.WriteLine($"Raw Data: {rawData}");

            var computedSignature = ComputeHmacSha256(rawData, _options.Value.SecretKey);

           // Console.WriteLine($"Computed Signature: {computedSignature}");
           // Console.WriteLine($"Provided Signature: {providedSignature}");

            return computedSignature.Equals(providedSignature, StringComparison.OrdinalIgnoreCase);
        }




        private string ComputeHmacSha256(string message, string secretKey)
        {
            var keyBytes = Encoding.UTF8.GetBytes(secretKey);
            var messageBytes = Encoding.UTF8.GetBytes(message);

            using (var hmac = new HMACSHA256(keyBytes))
            {
                var hashBytes = hmac.ComputeHash(messageBytes);
                return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
            }
        }

      
    }
}