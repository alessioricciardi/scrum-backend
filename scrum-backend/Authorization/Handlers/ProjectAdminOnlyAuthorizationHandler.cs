using Microsoft.AspNetCore.Authorization;
using scrum_backend.Authorization.Requirements;
using scrum_backend.Models.Projects;
using System.Security.Claims;

namespace scrum_backend.Authorization.Handlers
{
    public class ProjectAdminOnlyAuthorizationHandler : AuthorizationHandler<ProjectAdminOnlyRequirement, Project>
    {
        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, ProjectAdminOnlyRequirement requirement, Project project)
        {
            var userId = context.User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId is null) return;

            if (project.OwnerId == userId) context.Succeed(requirement);
        }
    }
}
