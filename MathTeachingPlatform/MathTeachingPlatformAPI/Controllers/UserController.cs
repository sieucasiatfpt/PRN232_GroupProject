using Application.Interfaces;
using Domain.Enum;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MathTeachingPlatformAPI.Controllers
{

    [ApiController]
    [Route("users")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

       // [Authorize(Roles = "Teacher,Admin")]
        [HttpPut("{id}/payment-status")]
        public async Task<IActionResult> UpdatePaymentStatus(int id, [FromBody] PaymentStatus paymentStatus)
        {
            var result = await _userService.UpdatePaymentStatusAsync(id, paymentStatus);

            if (!result)
            {
                return NotFound(new { message = "User not found or update failed." });
            }

            return Ok(new { message = "Payment status updated successfully." });
        }
        [Authorize(Roles = "Teacher,Admin")]
        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _userService.GetAllUsersAsync();
            return Ok(users);
        }
    }
}
