using Microsoft.AspNetCore.Identity;
using scrum_backend.Models.Projects;

namespace scrum_backend.Models.AppUsers;
public class AppUser : IdentityUser
{

    public required string Name { get; set; }

    public required string Surname { get; set; }

    public ICollection<ProjectMember> ProjectMemberships { get; set; } = [];
}
