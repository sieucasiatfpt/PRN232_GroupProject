using System.ComponentModel.DataAnnotations;
using Application.Validation;

namespace Application.DTOs.Auth
{
    public class RegisterRequest
    {
        [Required(ErrorMessage = "Username is required")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Username must be 3-50 characters")]
        public string Username { get; set; } = null!;

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        [StringLength(255, ErrorMessage = "Email cannot exceed 255 characters")]
        public string Email { get; set; } = null!;

        [Required(ErrorMessage = "Password is required")]
        [PasswordComplexity(MinLength = 8, RequireDigit = true, RequireUppercase = true, RequireLowercase = true)]
        public string Password { get; set; } = null!;

        [Required(ErrorMessage = "Role is required")]
        [RegularExpression("Teacher|Student|Admin", ErrorMessage = "Role must be Teacher, Student or Admin")]
        public string Role { get; set; } = "Student";
    }
}
