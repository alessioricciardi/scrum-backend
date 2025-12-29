using OneOf;
using scrum_backend.Dtos.Auth.Requests;
using scrum_backend.Results;

namespace scrum_backend.Services.AuthService
{
    public interface IAuthService
    {
        Task<OneOf<RegistrationSucceeded, RegistrationFailed, UserAlreadyExists>> RegisterAsync(RegisterRequestDto registerRequestDto);

        Task<OneOf<LoginSucceeded, LoginFailed>> LoginAsync(LoginRequestDto loginRequestDto);

        Task LogoutAsync();
    }
}


