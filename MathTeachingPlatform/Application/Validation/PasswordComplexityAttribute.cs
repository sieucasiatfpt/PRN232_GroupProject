using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace Application.Validation
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class PasswordComplexityAttribute : ValidationAttribute
    {
        public int MinLength { get; set; } = 8;
        public bool RequireDigit { get; set; } = true;
        public bool RequireUppercase { get; set; } = true;
        public bool RequireLowercase { get; set; } = true;
        public bool RequireNonAlphanumeric { get; set; } = false;

        public override bool IsValid(object? value)
        {
            var s = value as string;
            if (string.IsNullOrEmpty(s))
            {
                ErrorMessage = $"Password is required and must be at least {MinLength} characters.";
                return false;
            }

            if (s.Length < MinLength)
            {
                ErrorMessage = $"Password must be at least {MinLength} characters long.";
                return false;
            }

            if (RequireDigit && !s.Any(char.IsDigit))
            {
                ErrorMessage = "Password must contain at least one digit.";
                return false;
            }

            if (RequireUppercase && !s.Any(char.IsUpper))
            {
                ErrorMessage = "Password must contain at least one uppercase letter.";
                return false;
            }

            if (RequireLowercase && !s.Any(char.IsLower))
            {
                ErrorMessage = "Password must contain at least one lowercase letter.";
                return false;
            }

            if (RequireNonAlphanumeric && !Regex.IsMatch(s, @"\W"))
            {
                ErrorMessage = "Password must contain at least one non-alphanumeric character.";
                return false;
            }

            return true;
        }
    }
}
