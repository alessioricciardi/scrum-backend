using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using scrum_backend.Authorization.Requirements;
using scrum_backend.Data;
using scrum_backend.Models.Projects;
using System.Security.Claims;

namespace scrum_backend.Authorization.Handlers
{
    public class ProjectMemberOnlyAuthorizationHandler : AuthorizationHandler<ProjectMemberOnlyRequirement, Project>
    {
        private readonly AppDbContext _appDbContext;

        public ProjectMemberOnlyAuthorizationHandler(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        protected override async Task HandleRequirementAsync(
            AuthorizationHandlerContext context, ProjectMemberOnlyRequirement requirement, Project project)
        {
            var userId = context.User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId is null) return;

            var isMember = await _appDbContext.ProjectMembers
                                    .AnyAsync(pm => 
                                        pm.UserId == userId && 
                                        pm.ProjectId == project.Id
                                    );

            if (isMember) context.Succeed(requirement);
        }
    }
}
