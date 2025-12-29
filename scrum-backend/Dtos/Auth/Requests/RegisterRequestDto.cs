using System.ComponentModel.DataAnnotations;

namespace scrum_backend.Dtos.Auth.Requests
{
    public class RegisterRequestDto
    {
        [Required]
        public string UserName { get; set; } = default!;

        [Required]
        public string Password { get; set; } = default!;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = default!;

        [Required]
        public string Name { get; set; } = default!;

        [Required]
        public string Surname { get; set; } = default!;


    }
}
