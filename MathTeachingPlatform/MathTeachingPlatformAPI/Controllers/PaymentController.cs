using Application.DTOs.Payment;
using Application.Interfaces;
using Application.Models.Payment;
using Domain.Enum;
using Microsoft.AspNetCore.Mvc;

namespace MathTeachingPlatformAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
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

        [HttpPost]
        public async Task<IActionResult> CreatePayment([FromBody] CreatePaymentDto createPaymentDto)
        {
            try
            {
                var result = await _paymentService.CreatePaymentWithMomoAsync(createPaymentDto);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Error creating payment", Error = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPayment(int id)
        {
            try
            {
                var payment = await _paymentService.GetPaymentByIdAsync(id);
                if (payment == null)
                    return NotFound(new { Message = "Payment not found" });

                return Ok(payment);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Error retrieving payment", Error = ex.Message });
            }
        }

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
                return StatusCode(500, new { Message = "Error retrieving payments", Error = ex.Message });
            }
        }

        [HttpGet("teacher/{teacherId}")]
        public async Task<IActionResult> GetPaymentsByTeacher(int teacherId)
        {
            try
            {
                var payments = await _paymentService.GetPaymentsByTeacherIdAsync(teacherId);
                return Ok(payments);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Error retrieving payments", Error = ex.Message });
            }
        }

        [HttpGet("status/{status}")]
        public async Task<IActionResult> GetPaymentsByStatus(PaymentStatus status)
        {
            try
            {
                var payments = await _paymentService.GetPaymentsByStatusAsync(status);
                return Ok(payments);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Error retrieving payments", Error = ex.Message });
            }
        }

        [HttpPut]
        public async Task<IActionResult> UpdatePayment([FromBody] UpdatePaymentDto updatePaymentDto)
        {
            try
            {
                var updatedPayment = await _paymentService.UpdatePaymentAsync(updatePaymentDto);
                if (updatedPayment == null)
                    return NotFound(new { Message = "Payment not found" });

                return Ok(updatedPayment);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Error updating payment", Error = ex.Message });
            }
        }

        [HttpPatch("{id}/status")]
        public async Task<IActionResult> UpdatePaymentStatus(int id, [FromBody] PaymentStatus status)
        {
            try
            {
                // Create a DTO with only the PaymentId and Status
                var updatePaymentDto = new UpdatePaymentDto
                {
                    PaymentId = id,
                    Status = status
                };

                // Call the service to update the payment status
                var updatedPayment = await _paymentService.UpdatePaymentAsync(updatePaymentDto);

                if (updatedPayment == null)
                    return NotFound(new { Message = "Payment not found" });

                return Ok(new { Message = "Payment status updated successfully", Payment = updatedPayment });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Error updating payment status", Error = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePayment(int id)
        {
            try
            {
                var result = await _paymentService.DeletePaymentAsync(id);
                if (!result)
                    return NotFound(new { Message = "Payment not found" });

                return Ok(new { Message = "Payment deleted successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Error deleting payment", Error = ex.Message });
            }
        }

        // MOMO Integration (Existing methods)

        [HttpPost("CreatePaymentUrl")]
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
                return StatusCode(500, new { Message = "Internal server error", Error = ex.Message });
            }
        }

        [HttpGet("PaymentCallback")]
        public async Task<IActionResult> PaymentCallback()
        {
            try
            {
                // Access the query parameters directly from HttpContext.Request.Query
                var query = HttpContext.Request.Query;

                // Validate and process the callback using the MoMo service
                var callbackResponse = await _momoService.HandlePaymentCallbackAsync(query);

                // Check the error code to determine the payment status
                if (callbackResponse.ErrorCode == 0)
                {
                    // Update the payment status to "Completed" in your system
                    var updateResult = await _paymentService.UpdatePaymentAsync(new UpdatePaymentDto
                    {
                        PaymentId = int.Parse(callbackResponse.OrderId), // Assuming OrderId maps to PaymentId
                        Status = PaymentStatus.Completed
                    });

                    if (updateResult == null)
                    {
                        return NotFound(new { success = false, message = "Payment not found" });
                    }

                    return Ok(new { success = true, message = "Payment successful", data = callbackResponse });
                }
                else
                {
                    // Update the payment status to "Failed" in your system
                    var updateResult = await _paymentService.UpdatePaymentAsync(new UpdatePaymentDto
                    {
                        PaymentId = int.Parse(callbackResponse.OrderId),
                        Status = PaymentStatus.Failed
                    });

                    if (updateResult == null)
                    {
                        return NotFound(new { success = false, message = "Payment not found" });
                    }

                    return BadRequest(new { success = false, message = callbackResponse.Message, errorCode = callbackResponse.ErrorCode });
                }
            }
            catch (Exception ex)
            {
                // Log the exception and return an error response
                return StatusCode(500, new { success = false, message = "Internal server error", error = ex.Message });
            }
        }

        [HttpPost("MomoNotify")]
        public IActionResult MomoNotify()
        {
            var response = _momoService.PaymentExecuteAsync(Request.Query);
            // Process the payment notification here
            // Update order status, send confirmation emails, etc.
            return Ok("success");
        }
    }
}