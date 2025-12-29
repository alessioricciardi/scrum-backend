using System.ComponentModel.DataAnnotations;

namespace scrum_backend.Dtos.Project.Responses
{
    public class CreateProjectResponseDto
    {
        public required int Id { get; set; }
        public required string Name { get; set; }
        public required string Description { get; set; }

    }
}
