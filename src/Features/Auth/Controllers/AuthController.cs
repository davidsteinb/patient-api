using Microsoft.AspNetCore.Mvc;
using PatientApi.Features.Auth.DTOs;
using PatientApi.Features.Auth.Interfaces;

namespace PatientApi.Features.Auth.Controllers;

[ApiController]
[Route("api/v1/auth")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("login")]
    public IActionResult Login([FromBody] LoginRequestDto loginRequest)
    {
        var authResponse = _authService.Authenticate(loginRequest);
        if (authResponse == null)
            return Unauthorized(new { Message = "Invalid credentials" });

        return Ok(authResponse);
    }
}