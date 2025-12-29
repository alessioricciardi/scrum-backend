using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;

namespace scrum_backend.Dtos.Auth.Responses
{
    public class LoginResponseDto
    {
        public required string Id { get; set; }

        public required string Username { get; set; }

        public required string Name { get; set; }

        public required string Surname { get; set; }
    }

}
