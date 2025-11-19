using Application.DTOs.Auth;
using Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MathTeachingPlatformAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _svc;

        public AuthController(IUserService svc)
        {
            _svc = svc;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest req)
        {
            if (!ModelState.IsValid)
                return ValidationProblem(ModelState);

            try
            {
                var token = await _svc.RegisterAsync(req.Username, req.Email, req.Password, req.Role);
                return Ok(new
                {
                    token,
                    message = "Registration successful",
                    email = req.Email
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest req)
        {
            if (!ModelState.IsValid)
                return ValidationProblem(ModelState);

            try
            {
                var token = await _svc.LoginAsync(req.Email, req.Password);
                return Ok(new
                {
                    token,
                    message = "Login successful",
                    email = req.Email
                });
            }
            catch (Exception ex)
            {
                return Unauthorized(new { error = ex.Message });
            }
        }
    }
}
