using Microsoft.AspNetCore.Authorization;
using scrum_backend.Authorization.Requirements;
using scrum_backend.Models.Projects;
using System.Security.Claims;

namespace scrum_backend.Authorization.Handlers
{
    public class ProjectOwnerAuthorizationHandler : AuthorizationHandler<ProjectOwnerRequirement, Project>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, ProjectOwnerRequirement requirement, Project project)
        {
            var userId = context.User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId is null) return Task.CompletedTask;

            if (project.OwnerId == userId) context.Succeed(requirement);

            return Task.CompletedTask;
        }
    }
}
