using PatientApi.Features.Auth.DTOs;

namespace PatientApi.Features.Auth.Interfaces
{
    public interface IAuthService
    {
        AuthResponseDto Authenticate(LoginRequestDto loginRequestDto);
    }
}
