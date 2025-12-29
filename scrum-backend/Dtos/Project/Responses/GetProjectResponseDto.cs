using scrum_backend.Models.Projects;
using ProjectMemberType = scrum_backend.Models.Projects.ProjectMember;

namespace scrum_backend.Dtos.Project.Responses
{
    public class GetProjectResponseDto
    {
        public required int Id { get; set; }
        public required string Name { get; set; }
        public required string Description { get; set; }
        public required string OwnerId { get; set; }
    }
}
