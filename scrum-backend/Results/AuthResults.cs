using scrum_backend.Dtos.Auth.Responses;

namespace scrum_backend.Results
{
    public record UserAlreadyExists;

    public record RegistrationSucceeded;
    public record RegistrationFailed(IEnumerable<string> Errors);

    public record LoginSucceeded(LoginResponseDto LoginResponseDto);
    public record LoginFailed;

}
