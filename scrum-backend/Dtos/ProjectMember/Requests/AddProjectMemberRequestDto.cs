using scrum_backend.Models.Projects;
using System.ComponentModel.DataAnnotations;

namespace scrum_backend.Dtos.ProjectMember.Requests
{ 
    public class AddProjectMemberRequestDto
    {
        [Required]
        public string UserId { get; set; } = default!;

        [EnumDataType(typeof(ProjectMemberRole))]
        public ProjectMemberRole Role { get; set; }
    }
}
