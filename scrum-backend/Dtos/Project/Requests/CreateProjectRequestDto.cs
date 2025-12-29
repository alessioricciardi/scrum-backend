using System.ComponentModel.DataAnnotations;

namespace scrum_backend.Dtos.Project.Requests
{
    public class CreateProjectRequestDto
    {
        [Required]
        [MinLength(5)]
        public string Name { get; set; } = default!;

        public string Description { get; set; } = "Newly created Scrum Project.";

    }
}
    