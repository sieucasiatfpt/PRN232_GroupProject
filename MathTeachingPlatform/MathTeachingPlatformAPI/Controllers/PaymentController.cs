using Application.DTOs.Payment;
using Application.Interfaces;
using Application.Models.Payment;
using Domain.Enum;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MathTeachingPlatformAPI.Controllers
{
    [ApiController]
    [Route("payments")]
    public class PaymentController : ControllerBase
    {
        private readonly IMomoService _momoService;
        private readonly IPaymentService _paymentService;

        public PaymentController(IMomoService momoService, IPaymentService paymentService)
        {
            _momoService = momoService;
            _paymentService = paymentService;
        }

        // CRUD Operations
        [Authorize(Roles = "Teacher,Admin")]
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
        [Authorize(Roles = "Teacher,Admin")]
        [HttpGet("callback")]
        public async Task<IActionResult> PaymentCallback([FromQuery] string? url = null)
        {
            try
            {
                // If a full URL is provided, extract the query string from it
                var parameters = string.IsNullOrEmpty(url)
                    ? HttpContext.Request.Query.ToDictionary(kvp => kvp.Key, kvp => kvp.Value)
                    : Microsoft.AspNetCore.WebUtilities.QueryHelpers.ParseQuery(new Uri(url).Query);

                // Map query parameters to the callback response model
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

                // Validate the signature
                string signature = parameters["signature"];
                var isValidSignature = await _momoService.ValidateSignature(callback, signature);
                if (!isValidSignature)
                    return BadRequest(new { success = false, message = "Invalid signature" });

                // Extract payment ID from ExtraData
                if (!int.TryParse(callback.ExtraData, out var paymentId))
                    return BadRequest(new { success = false, message = "Invalid PaymentId in ExtraData" });

                // Determine payment status
                var status = callback.ErrorCode == 0 ? PaymentStatus.Completed : PaymentStatus.Failed;

                // Update payment status
                var updateResult = await _paymentService.UpdatePaymentAsync(new UpdatePaymentDto
                {
                    PaymentId = paymentId,
                    Status = status
                });

                // Return appropriate response
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