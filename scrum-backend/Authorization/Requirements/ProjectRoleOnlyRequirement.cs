using Microsoft.AspNetCore.Authorization;
using scrum_backend.Models.Projects;

namespace scrum_backend.Authorization.Requirements
{
    public class ProjectRoleOnlyRequirement : IAuthorizationRequirement
    {
        public ProjectMemberRole RequiredRole { get; }

        public ProjectRoleOnlyRequirement(ProjectMemberRole requiredRole)
        {
            RequiredRole = requiredRole;
        }
    }
}