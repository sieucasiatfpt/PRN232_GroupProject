using Domain.Entities;
using Domain.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IJwtService
    {
        string GenerateAccessToken(User user);
        string GenerateRefreshToken();
        ClaimsPrincipal? ValidateToken(string token);
        ClaimsPrincipal? ValidateExpiredToken(string token);
        int GetAccessTokenExpirationMinutes();
        int GetRefreshTokenExpirationDays();
    }
}
