using System.ComponentModel.DataAnnotations;

namespace scrum_backend.Dtos.Auth.Requests
{
    public class LoginRequestDto
    {
        [Required]
        [MinLength(5)]
        public string UserName { get; set; } = default!;

        [Required]
        public string Password { get; set; } = default!;
    }
}
