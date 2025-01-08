using System.Security.Claims;
using ControlWork_9.Model;

namespace WebApi.Interfaces;

public interface IJwtProvider
{
    string GenerateUserToken(UserModel user);
    string GenerateRefreshToken();
    bool ValidateToken(string token);
    ClaimsPrincipal? GetPrincipalFromToken(string token);

}