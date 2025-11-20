using Application.DTOs.Auth;
using Application.Interfaces;
using Domain.Enum;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace MathTeachingPlatformAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IJwtService _jwtService;
        private readonly IConfiguration _configuration;

        public AuthController(IUserService userService, IJwtService jwtService, IConfiguration configuration)
        {
            _userService = userService;
            _jwtService = jwtService;
            _configuration = configuration;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest req)
        {
            if (!ModelState.IsValid)
                return ValidationProblem(ModelState);

            try
            {
                var response = await _userService.RegisterAsync(req.Username, req.Email, req.Password, req.Role);

                return Ok(response);
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
                var response = await _userService.LoginAsync(req.Email, req.Password);

                // Generate and set refresh token as HTTP-only cookie
                var refreshToken = _jwtService.GenerateRefreshToken();
                SetRefreshTokenCookie(refreshToken);

                return Ok(response);
            }
            catch (Exception ex)
            {
                return Unauthorized(new { error = ex.Message });
            }
        }

        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken()
        {
            try
            {
                // Get refresh token from cookie
                var refreshToken = Request.Cookies["refreshToken"];

                if (string.IsNullOrEmpty(refreshToken))
                {
                    return Unauthorized(new { error = "Refresh token not found" });
                }

                // Get user ID from session or from an access token if provided
                var accessToken = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

                if (string.IsNullOrEmpty(accessToken))
                {
                    return Unauthorized(new { error = "Access token required for refresh" });
                }

                // Validate expired token to get user info
                var principal = _jwtService.ValidateExpiredToken(accessToken);

                if (principal == null)
                {
                    return Unauthorized(new { error = "Invalid access token" });
                }

                // Extract user ID from claims
                var userIdClaim = principal.FindFirst(ClaimTypes.NameIdentifier)?.Value
                    ?? principal.FindFirst("sub")?.Value;
                var email = principal.FindFirst(ClaimTypes.Email)?.Value
                    ?? principal.FindFirst("email")?.Value;
                var role = principal.FindFirst(ClaimTypes.Role)?.Value;

                if (string.IsNullOrEmpty(userIdClaim))
                {
                    return Unauthorized(new { error = "Invalid token claims" });
                }

                // Generate new access token
                var newAccessToken = _jwtService.GenerateAccessToken(new Domain.Entities.User
                {
                    UserId = int.Parse(userIdClaim),
                    Email = email,
                    Role = Enum.Parse<Domain.Enum.UserRole>(role ?? "Student")
                });

                // Generate and set new refresh token
                var newRefreshToken = _jwtService.GenerateRefreshToken();
                SetRefreshTokenCookie(newRefreshToken);

                return Ok(new AuthResponse
                {
                    AccessToken = newAccessToken,
                    AccessTokenExpiry = DateTime.UtcNow.AddMinutes(_jwtService.GetAccessTokenExpirationMinutes()),
                    Email = email ?? string.Empty,
                    Role = role ?? string.Empty,
                    Message = "Token refreshed successfully"
                });
            }
            catch (Exception ex)
            {
                return Unauthorized(new { error = ex.Message });
            }
        }

        [Authorize]
        [HttpPost("logout")]
        public IActionResult Logout()
        {
            // Clear the refresh token cookie
            Response.Cookies.Delete("refreshToken");

            return Ok(new { message = "Logged out successfully" });
        }

        [Authorize]
        [HttpGet("me")]
        public IActionResult GetCurrentUser()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                ?? User.FindFirst("sub")?.Value;
            var email = User.FindFirst(ClaimTypes.Email)?.Value
                ?? User.FindFirst("email")?.Value;
            var role = User.FindFirst(ClaimTypes.Role)?.Value;
            var username = User.FindFirst("username")?.Value;

            return Ok(new
            {
                userId = userIdClaim,
                email,
                role,
                username
            });
        }

        private void SetRefreshTokenCookie(string refreshToken)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true, // Prevents JavaScript access
                Secure = true, // Only sent over HTTPS
                SameSite = SameSiteMode.Strict, // CSRF protection
                Expires = DateTime.UtcNow.AddDays(_jwtService.GetRefreshTokenExpirationDays()),
                Path = "/",
                IsEssential = true
            };

            Response.Cookies.Append("refreshToken", refreshToken, cookieOptions);
        }
    }
}
