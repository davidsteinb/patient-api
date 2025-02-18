using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using PatientApi.Features.Auth.DTOs;
using PatientApi.Features.Auth.Interfaces;

namespace PatientApi.Features.Auth.Services;

public class AuthService : IAuthService
{    
    private readonly string _jwtSecret;
    private readonly string _appPassword;

    public AuthService()
    {
        _jwtSecret = Environment.GetEnvironmentVariable("JWT_SECRET")
                     ?? throw new InvalidOperationException("Missing JWT_SECRET!");
        _appPassword = Environment.GetEnvironmentVariable("APP_PASSWORD")
                      ?? throw new InvalidOperationException("Missing APP_PASSWORD!");
    }

    public AuthResponseDto Authenticate(LoginRequestDto loginRequest)
    {        
        if (loginRequest.Username.ToLower() != "admin" || loginRequest.Password != _appPassword)
            return null;

        return GenerateJwtToken();
    }

    private AuthResponseDto GenerateJwtToken()
    {
        var key = Encoding.UTF8.GetBytes(_jwtSecret);
        var tokenHandler = new JwtSecurityTokenHandler();
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.Name, "admin")
            }),            
            Expires = DateTime.UtcNow.AddHours(2),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return new AuthResponseDto { Token = tokenHandler.WriteToken(token) };
    }
}