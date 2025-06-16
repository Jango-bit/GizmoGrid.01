using GizmoGrid._01.Dto;

namespace GizmoGrid._01.Services.AuthServices
{
    public interface IAuthService
    {
        Task<string> RegisterAsync(UserRegisterDto dto);
        Task<AuthenticatedUserDto> LoginAsync(UserLoginDto dto);
    }
}
