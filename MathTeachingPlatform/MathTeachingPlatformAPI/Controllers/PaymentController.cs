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
        [Authorize(Roles = "Teacher,Admin")]
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
                // 1️⃣ Parse query parameters
                var parameters = string.IsNullOrEmpty(url)
                    ? HttpContext.Request.Query.ToDictionary(kvp => kvp.Key, kvp => kvp.Value)
                    : Microsoft.AspNetCore.WebUtilities.QueryHelpers.ParseQuery(new Uri(url).Query);

                // 2️⃣ Build callback model
                var callback = new MomoCallbackResponseModel
                {
                    PartnerCode = parameters.ContainsKey("partnerCode") ? parameters["partnerCode"].ToString() ?? "" : "",
                    AccessKey = parameters.ContainsKey("accessKey") ? parameters["accessKey"].ToString() ?? "" : "",
                    RequestId = parameters.ContainsKey("requestId") ? parameters["requestId"].ToString() ?? "" : "",
                    Amount = parameters.ContainsKey("amount") ? parameters["amount"].ToString() ?? "" : "",
                    OrderId = parameters.ContainsKey("orderId") ? parameters["orderId"].ToString() ?? "" : "",
                    OrderInfo = parameters.ContainsKey("orderInfo") ? parameters["orderInfo"].ToString() ?? "" : "",
                    OrderType = parameters.ContainsKey("orderType") ? parameters["orderType"].ToString() ?? "" : "",
                    TransId = parameters.ContainsKey("transId") ? parameters["transId"].ToString() ?? "" : "",
                    Message = parameters.ContainsKey("message") ? parameters["message"].ToString() ?? "" : "",
                    LocalMessage = parameters.ContainsKey("localMessage") ? parameters["localMessage"].ToString() ?? "" : "",
                    ResponseTime = parameters.ContainsKey("responseTime") ? parameters["responseTime"].ToString() ?? "" : "",
                    ErrorCode = parameters.ContainsKey("errorCode") && int.TryParse(parameters["errorCode"], out var errorCode) ? errorCode : -1,
                    PayType = parameters.ContainsKey("payType") ? parameters["payType"].ToString() ?? "" : "",
                    ExtraData = parameters.ContainsKey("extraData") ? parameters["extraData"].ToString() ?? "" : ""
                };

                // 3️⃣ Validate signature
                if (!parameters.ContainsKey("signature"))
                    return BadRequest(new { success = false, error = "Missing signature parameter" });

                var signature = parameters["signature"];
                if (!await _momoService.ValidateSignature(callback, signature))
                    return BadRequest(new { success = false, error = "Invalid signature" });

                // 4️⃣ Parse PaymentId from ExtraData
                if (string.IsNullOrWhiteSpace(callback.ExtraData))
                    return BadRequest(new { success = false, error = "Missing PaymentId in ExtraData" });

                if (!int.TryParse(callback.ExtraData, out var paymentId))
                    return BadRequest(new { success = false, error = "Invalid PaymentId in ExtraData" });

                // 5️⃣ Get Payment
                var payment = await _paymentService.GetPaymentByIdAsync(paymentId);
                if (payment == null)
                    return NotFound(new { success = false, error = "Payment not found" });

                var teacherId = payment.TeacherId;

                // 6️⃣ Get Teacher
                TeacherIdDto? teacherData = null;
                var teacherApiUrl = $"https://mathweb-e9ezeegehmfddmdp.canadacentral-01.azurewebsites.net/teachers/{teacherId}";
                using (var httpClient = new HttpClient())
                {
                    var teacherResponse = await httpClient.GetAsync(teacherApiUrl);
                    if (!teacherResponse.IsSuccessStatusCode)
                    {
                        var detail = await teacherResponse.Content.ReadAsStringAsync();
                        return NotFound(new { success = false, error = "Teacher not found", teacherId, detail });
                    }
                    var json = await teacherResponse.Content.ReadAsStringAsync();
                    teacherData = JsonSerializer.Deserialize<TeacherIdDto>(json, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });
                    if (teacherData == null)
                        return BadRequest(new { success = false, error = "Failed to deserialize teacher data", teacherId, json });
                }
                var userId = teacherData.UserId;

                // 7️⃣ Determine payment status
                var status = callback.ErrorCode == 0 ? PaymentStatus.Completed : PaymentStatus.Failed;

                // 8️⃣ Update Payment
                await _paymentService.UpdatePaymentAsync(new UpdatePaymentDto
                {
                    PaymentId = paymentId,
                    Status = status
                });

                // 9️⃣ Update User PaymentStatus via UserController API
                var userApiUrl = $"https://mathweb-e9ezeegehmfddmdp.canadacentral-01.azurewebsites.net/users/{userId}/payment-status";
                using (var httpClient = new HttpClient())
                {
                    var content = new StringContent(
                        JsonSerializer.Serialize(status.ToString()),
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
                            error = "Failed to update user payment status",
                            detail
                        });
                    }
                }

                // 🔟 Return callback result
                return status == PaymentStatus.Completed
                    ? Ok(new { success = true, message = "Payment successful", data = callback })
                    : BadRequest(new { success = false, error = callback.Message, errorCode = callback.ErrorCode });
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