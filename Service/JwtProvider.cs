﻿using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using ControlWork_9.Model;
using ControlWork_9.Options;
using Microsoft.IdentityModel.Tokens;

using WebApi.Interfaces;

namespace ControlWork_9.Service;

public class JwtProvider : IJwtProvider
{
    public string GenerateUserToken(UserModel user)
    {
        var now = DateTime.UtcNow;
        string role;
      

        Claim[] claims =
        {
            new("userId", user.Id.ToString())
        };
        
        var jwt = new JwtSecurityToken(
            issuer: AuthOptions.Issuer,
            audience: AuthOptions.Audience,
            notBefore: now,
            claims: claims,
            expires: now.Add(TimeSpan.FromMinutes(AuthOptions.Lifetime)),
            signingCredentials: new SigningCredentials(
                AuthOptions.GetSymmetricSecurityKey(),
                SecurityAlgorithms.HmacSha256)
        );
        
        var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

        return encodedJwt;
    }
    
    public string GenerateRefreshToken()
    {
        var randomNumber = new byte[64];
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(randomNumber);
        }
        return Convert.ToBase64String(randomNumber);
    }
    
    public ClaimsPrincipal? GetPrincipalFromToken(string token)
    {
        var securityKey = AuthOptions.GetSymmetricSecurityKey();
        var validation = new TokenValidationParameters
        {
            IssuerSigningKey = securityKey,
            ValidateLifetime = false,
            ValidateAudience = false,
            ValidateIssuer = false,
            ValidateActor = false
        };
        return new JwtSecurityTokenHandler().ValidateToken(token, validation, out _);
    }
    
    public bool ValidateToken(string token)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        try
        {
            tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = AuthOptions.GetSymmetricSecurityKey(),
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = true
            }, out SecurityToken validatedToken);

            return validatedToken != null && validatedToken.ValidTo > DateTime.UtcNow;
        }
        catch
        {
            return false;
        }
    }
    public Guid GetUserIdFromToken(ClaimsPrincipal user)
    {
        var userIdClaim = user.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
        return userIdClaim != null ? Guid.Parse(userIdClaim.Value) : Guid.Empty;
    }
}
