using scrum_backend.Models.Projects;

namespace scrum_backend.Dtos.ProjectMember.Responses
{
    public class GetProjectMemberResponseDto
    {
        public required int Id;
        public required string UserId;
        public required ProjectMemberRole Role;
    }
}
