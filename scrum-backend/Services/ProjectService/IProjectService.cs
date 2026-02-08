using OneOf;
using scrum_backend.Results;
using scrum_backend.Dtos.Project.Requests;
using System.Security.Claims;

namespace scrum_backend.Services.ProjectService
{
    public interface IProjectService
    {
        Task<OneOf<GetProjectSucceeded, ProjectNotFound>> GetProjectByIdAsync(int projectId);
        Task<OneOf<CreateProjectSucceeded, CreateProjectFailed, UserNotFound, Unauthorized>> CreateProjectAsync(CreateProjectRequestDto createProjectDto, ClaimsPrincipal user);
        Task<OneOf<UpdateProjectSucceeded, UpdateProjectFailed, ProjectNotFound, Forbidden>> UpdateProjectAsync(int projectId, UpdateProjectRequestDto updateProjectDto, ClaimsPrincipal user);
        Task<OneOf<DeleteProjectSucceeded, DeleteProjectFailed, ProjectNotFound, Forbidden>> DeleteProjectAsync(int projectId, ClaimsPrincipal user);
    }
}
