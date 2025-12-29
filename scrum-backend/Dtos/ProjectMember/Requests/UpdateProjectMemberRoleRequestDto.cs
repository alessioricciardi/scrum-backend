using scrum_backend.Models.Projects;
using System.ComponentModel.DataAnnotations;

namespace scrum_backend.Dtos.ProjectMember.Requests
{
    public class UpdateProjectMemberRoleRequestDto
    {
        [EnumDataType(typeof(ProjectMemberRole))]
        public ProjectMemberRole Role { get; set; }
    }
}
