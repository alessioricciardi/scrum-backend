using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using OneOf;
using scrum_backend.Authorization.Policies;
using scrum_backend.Data;
using scrum_backend.Models.Projects;
using scrum_backend.Results;
using System.Security.Claims;

namespace scrum_backend.Authorization.Services
{
    public class ProjectAccessService : IProjectAccessService
    {
        private readonly AppDbContext _appDbContext;
        private readonly IAuthorizationService _authorizationService;

        public ProjectAccessService(IAuthorizationService authorizationService, AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
            _authorizationService = authorizationService;
        }

        public async Task<OneOf<Project, ProjectNotFound, Forbidden>> GetAccessAsync(int projectId, ClaimsPrincipal user, string policy)
        {
            var project = await _appDbContext.Projects
                           .FirstOrDefaultAsync(p => p.Id == projectId);

            if (project is null)
                return new ProjectNotFound();

            var authResult = await _authorizationService.AuthorizeAsync(user, project, policy);
            if (!authResult.Succeeded)
                return new Forbidden();

            return project;
        }
    }
}
