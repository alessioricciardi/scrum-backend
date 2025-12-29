using OneOf;
using scrum_backend.Results;
using scrum_backend.Dtos.Project.Requests;
using System.Security.Claims;

namespace scrum_backend.Services.ProjectService
{
    public interface IProjectService
    {
        Task<OneOf<ProjectGetSucceeded, ProjectNotFound>> GetProjectByIdAsync(int projectId);
        Task<OneOf<ProjectCreateSucceeded, ProjectCreateFailed, UserNotFound, Unauthorized>> CreateProjectAsync(CreateProjectRequestDto createProjectDto, ClaimsPrincipal user);
        Task<OneOf<ProjectUpdateSucceeded, ProjectUpdateFailed, ProjectNotFound, Forbidden>> UpdateProjectAsync(int projectId, UpdateProjectRequestDto updateProjectDto, ClaimsPrincipal user);
        Task<OneOf<ProjectDeleteSucceeded, ProjectDeleteFailed, ProjectNotFound, Forbidden>> DeleteProjectAsync(int projectId, ClaimsPrincipal user);
    }
}
