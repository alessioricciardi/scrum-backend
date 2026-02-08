using Microsoft.AspNetCore.Mvc;
using OneOf;
using scrum_backend.Dtos;
using scrum_backend.Dtos.Auth.Requests;
using scrum_backend.Results;
using scrum_backend.Services.AuthService;

namespace scrum_backend.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterAsync([FromBody] RegisterRequestDto registerRequestDto)
        {
            var result = await _authService.RegisterAsync(registerRequestDto);

            return result.Match<IActionResult>(
                registrationSucceeded => StatusCode(201, new { Message = "Registration succeeded." }),
                registrationFailed => BadRequest(new ErrorResponseDto
                {
                    Message = "Registration failed.",
                    Errors = registrationFailed.Errors
                }),
                userAlreadyExists => Conflict(new ErrorResponseDto{ Message = "User already exists." })
            );
        }

        [HttpPost("login")]
        public async Task<IActionResult> LoginAsync([FromBody] LoginRequestDto loginRequestDto)
        {
            var result = await _authService.LoginAsync(loginRequestDto);

            return result.Match<IActionResult>(
                loginSucceeded => Ok(loginSucceeded.LoginResponseDto),
                loginFailed => Unauthorized(new ErrorResponseDto{ Message = "Invalid username or password." })
            );
        }

        [HttpPost("logout")]
        public async Task<IActionResult> LogoutAsync()
        {
            await _authService.LogoutAsync();

            return NoContent();
        }
    }


}
