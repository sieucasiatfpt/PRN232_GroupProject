using Application.DTOs.Payment;
using Application.DTOs.Teacher;
using Application.Interfaces;
using Application.Models.Payment;
using Domain.Enum;
using Infrastructure.ApiClients;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Text;
using System.Text.Json;

namespace MathTeachingPlatformAPI.Controllers
{
    [ApiController]
    [Route("payments")]
    public class PaymentController : ControllerBase
    {
        private readonly IMomoService _momoService;
        private readonly IPaymentService _paymentService;
        private readonly ITeacherApiClient _teacherApiClient;

        public PaymentController(IMomoService momoService, IPaymentService paymentService, ITeacherApiClient teacherApiClient)
        {
            _momoService = momoService;
            _paymentService = paymentService;
            _teacherApiClient = teacherApiClient; // Assign the injected dependency
        }

        // CRUD Operations
        // [Authorize(Roles = "Teacher,Admin")]
        [HttpPost]
        public async Task<IActionResult> CreatePayment([FromBody] CreatePaymentDto createPaymentDto)
        {
            if (!ModelState.IsValid)
                return ValidationProblem(ModelState);

            try
            {
                var result = await _paymentService.CreatePaymentWithMomoAsync(createPaymentDto);
                return CreatedAtAction(nameof(GetPayment), new { id = result.PaymentId }, result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
        [Authorize(Roles = "Teacher,Admin")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetPayment(int id)
        {
            try
            {
                var payment = await _paymentService.GetPaymentByIdAsync(id);
                if (payment == null)
                    return NotFound(new { error = "Payment not found" });

                return Ok(payment);
            }
            catch (Exception ex)
            {
                return NotFound(new { error = ex.Message });
            }
        }
        [Authorize(Roles = "Teacher,Admin")]
        [HttpGet]
        public async Task<IActionResult> GetAllPayments()
        {
            try
            {
                var payments = await _paymentService.GetAllPaymentsAsync();
                return Ok(payments);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
        [Authorize(Roles = "Teacher,Admin")]
        [HttpGet("by-teacher/{teacherId}")]
        public async Task<IActionResult> GetPaymentsByTeacher(int teacherId)
        {
            try
            {
                var payments = await _paymentService.GetPaymentsByTeacherIdAsync(teacherId);
                return Ok(payments);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
        [Authorize(Roles = "Teacher,Admin")]
        [HttpGet("by-status/{status}")]
        public async Task<IActionResult> GetPaymentsByStatus(PaymentStatus status)
        {
            try
            {
                var payments = await _paymentService.GetPaymentsByStatusAsync(status);
                return Ok(payments);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
        [Authorize(Roles = "Teacher,Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePayment(int id, [FromBody] UpdatePaymentDto updatePaymentDto)
        {
            if (!ModelState.IsValid)
                return ValidationProblem(ModelState);

            try
            {
                updatePaymentDto.PaymentId = id;
                var updatedPayment = await _paymentService.UpdatePaymentAsync(updatePaymentDto);
                if (updatedPayment == null)
                    return NotFound(new { error = "Payment not found" });

                return Ok(updatedPayment);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
        [Authorize(Roles = "Teacher,Admin")]
        [HttpPatch("{id}/status")]
        public async Task<IActionResult> UpdatePaymentStatus(int id, [FromBody] PaymentStatus status)
        {
            try
            {
                var updatePaymentDto = new UpdatePaymentDto
                {
                    PaymentId = id,
                    Status = status
                };

                var updatedPayment = await _paymentService.UpdatePaymentAsync(updatePaymentDto);
                if (updatedPayment == null)
                    return NotFound(new { error = "Payment not found" });

                return Ok(new { message = "Payment status updated successfully", Payment = updatedPayment });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
        [Authorize(Roles = "Teacher,Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePayment(int id)
        {
            try
            {
                var result = await _paymentService.DeletePaymentAsync(id);
                if (!result)
                    return NotFound(new { error = "Payment not found" });

                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        // MOMO Integration
        //[Authorize(Roles = "Teacher,Admin")]
        [HttpPost("momo/url")]
        public async Task<IActionResult> CreatePaymentUrl([FromBody] OrderInfoModel model)
        {
            try
            {
                var response = await _momoService.CreatePaymentAsync(model);

                if (response.ErrorCode == 0)
                {
                    return Ok(new { PayUrl = response.PayUrl, OrderId = response.OrderId });
                }
                else
                {
                    return BadRequest(new { Message = response.Message, ErrorCode = response.ErrorCode });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
        // [Authorize(Roles = "Teacher,Admin")]
        [HttpGet("callback")]
        public async Task<IActionResult> PaymentCallback([FromQuery] string? url = null)
        {
            try
            {
                // 1️⃣ Lấy query parameters
                var parameters = string.IsNullOrEmpty(url)
                    ? HttpContext.Request.Query.ToDictionary(kvp => kvp.Key, kvp => kvp.Value)
                    : Microsoft.AspNetCore.WebUtilities.QueryHelpers.ParseQuery(new Uri(url).Query);

                var callback = new MomoCallbackResponseModel
                {
                    PartnerCode = parameters["partnerCode"],
                    AccessKey = parameters["accessKey"],
                    RequestId = parameters["requestId"],
                    Amount = parameters["amount"],
                    OrderId = parameters["orderId"],
                    OrderInfo = parameters["orderInfo"],
                    OrderType = parameters["orderType"],
                    TransId = parameters["transId"],
                    Message = parameters["message"],
                    LocalMessage = parameters["localMessage"],
                    ResponseTime = parameters["responseTime"],
                    ErrorCode = int.TryParse(parameters["errorCode"], out var errorCode) ? errorCode : -1,
                    PayType = parameters["payType"],
                    ExtraData = parameters.ContainsKey("extraData") ? parameters["extraData"].ToString() ?? "" : ""
                };

                // 2️⃣ Validate signature
                var signature = parameters["signature"];
                if (!await _momoService.ValidateSignature(callback, signature))
                    return BadRequest(new { success = false, message = "Invalid signature" });

                // 3️⃣ Lấy PaymentId từ ExtraData
                if (!int.TryParse(callback.ExtraData, out var paymentId))
                    return BadRequest(new { success = false, message = "Invalid PaymentId in ExtraData" });

                // 4️⃣ Lấy Payment để lấy TeacherId
                var payment = await _paymentService.GetPaymentByIdAsync(paymentId);
                if (payment == null)
                    return NotFound(new { success = false, message = "Payment not found" });

                var teacherId = payment.TeacherId;

                // 5️⃣ Lấy thông tin Teacher
                var teacherData = await _teacherApiClient.GetTeacherByIdAsync(teacherId);
                if (teacherData == null || teacherData.UserId == 0)
                    return BadRequest(new { success = false, message = "Failed to retrieve teacher details" });

                var userId = teacherData.UserId;

                // 6️⃣ Xác định trạng thái payment
                var status = callback.ErrorCode == 0 ? PaymentStatus.Completed : PaymentStatus.Failed;

                // 7️⃣ Cập nhật Payment
                await _paymentService.UpdatePaymentAsync(new UpdatePaymentDto
                {
                    PaymentId = paymentId,
                    Status = status
                });

                // 8️⃣ Cập nhật User PaymentStatus qua UserController API
                var payload = new { paymentStatus = (int)status }; // Gửi dạng int
                var userApiUrl = $"https://mathweb-e9ezeegehmfddmdp.canadacentral-01.azurewebsites.net/users/{userId}/payment-status";

                using (var httpClient = new HttpClient())
                {
                    var content = new StringContent(
                        JsonSerializer.Serialize(payload),
                        Encoding.UTF8,
                        "application/json"
                    );

                    var response = await httpClient.PutAsync(userApiUrl, content);
                    if (!response.IsSuccessStatusCode)
                    {
                        var detail = await response.Content.ReadAsStringAsync();
                        return BadRequest(new
                        {
                            success = false,
                            message = "Failed to update user payment status",
                            detail
                        });
                    }
                }

                // 9️⃣ Trả kết quả callback
                return status == PaymentStatus.Completed
                    ? Ok(new { success = true, message = "Payment successful", data = callback })
                    : BadRequest(new { success = false, message = callback.Message, errorCode = callback.ErrorCode });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, error = ex.Message });
            }
        }


        [Authorize(Roles = "Teacher,Admin")]
        [HttpPost("momo/notify")]
        public IActionResult MomoNotify()
        {
            var response = _momoService.PaymentExecuteAsync(Request.Query);
            return Ok("success");
        }
    }
}