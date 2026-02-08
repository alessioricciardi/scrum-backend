using OneOf;
using scrum_backend.Models.Projects;
using scrum_backend.Results;
using System.Security.Claims;

namespace scrum_backend.Authorization.Services
{
    public interface IProjectAccessService
    {
        public Task<OneOf<Project, ProjectNotFound, Forbidden>> GetAccessAsync(int projectId, ClaimsPrincipal user, string policy);
    }
}
