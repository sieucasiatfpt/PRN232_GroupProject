using Application.Interfaces;
using Domain.Enum;
using Microsoft.AspNetCore.Mvc;

namespace MathTeachingPlatformAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPatch("{id}/payment-status")]
        public async Task<IActionResult> UpdatePaymentStatus(int id, [FromBody] PaymentStatus paymentStatus)
        {
            var result = await _userService.UpdatePaymentStatusAsync(id, paymentStatus);

            if (!result)
            {
                return NotFound(new { message = "User not found or update failed." });
            }

            return Ok(new { message = "Payment status updated successfully." });
        }
    }
}