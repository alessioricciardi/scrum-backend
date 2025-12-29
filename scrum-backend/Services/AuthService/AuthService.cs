using OneOf;
using Microsoft.AspNetCore.Identity;
using scrum_backend.Dtos.Auth.Requests;
using scrum_backend.Dtos.Auth.Responses;
using scrum_backend.Models.AppUsers;
using scrum_backend.Results;
using AutoMapper;

namespace scrum_backend.Services.AuthService
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IMapper _mapper;
        private readonly ILogger<AuthService> _logger;

        public AuthService(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, IMapper mapper, ILogger<AuthService> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<OneOf<RegistrationSucceeded, RegistrationFailed, UserAlreadyExists>> RegisterAsync(RegisterRequestDto registerRequestDto)
        {
            _logger.LogInformation("Registration attempt started for user: {UserName}", registerRequestDto.UserName);

            var existingUser = await _userManager.FindByNameAsync(registerRequestDto.UserName);
            
            if(existingUser is not null)
            {
                _logger.LogWarning("Registration failed. User: {UserName} already exists.", registerRequestDto.UserName);
                return new UserAlreadyExists();
            }

            var appUser = new AppUser
            {
                UserName = registerRequestDto.UserName,
                Email = registerRequestDto.Email,
                Name = registerRequestDto.Name,
                Surname = registerRequestDto.Surname
            };

            var result = await _userManager.CreateAsync(appUser, registerRequestDto.Password);

            if (!result.Succeeded)
            {
                _logger.LogWarning("Registration failed. User: {UserName} could not be registered. Errors: {Errors}", registerRequestDto.UserName, result.Errors);

                var errors = result.Errors.Select((error) => error.Description); 

                return new RegistrationFailed(errors);
            }

            _logger.LogInformation("Registration succeeded. User: {UserName} has been registered.", registerRequestDto.UserName);
            return new RegistrationSucceeded();

        }
        public async Task<OneOf<LoginSucceeded, LoginFailed>> LoginAsync(LoginRequestDto loginRequestDto)
        {
            _logger.LogInformation("Login attempt started for user: {UserName}", loginRequestDto.UserName);

            var result = await _signInManager.PasswordSignInAsync(
                userName: loginRequestDto.UserName,
                password: loginRequestDto.Password,
                isPersistent: false,
                lockoutOnFailure: false
            );

            if (!result.Succeeded)
            {
                _logger.LogWarning("Login failed. User: {UserName} could not be logged in.", loginRequestDto.UserName);

                return new LoginFailed();
            }

            var user = await _userManager.FindByNameAsync(loginRequestDto.UserName);
            if (user is null)
            {
                _logger.LogError("User: {Username} found by SignInManager but not found by UserManager", loginRequestDto.UserName);
                return new LoginFailed();
            }

            _logger.LogInformation("Login succeeded. User {UserName} has logged in.", loginRequestDto.UserName);

            var loginResponse = _mapper.Map<LoginResponseDto>(user);
            return new LoginSucceeded(loginResponse);
        }

        public async Task LogoutAsync()
        {
            await _signInManager.SignOutAsync();
        }
    }
}
