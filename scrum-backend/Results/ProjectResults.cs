using scrum_backend.Dtos.Project.Responses;

namespace scrum_backend.Results
{
    public record ProjectNotFound;
    public record GetProjectSucceeded(GetProjectResponseDto GetProjectResponseDto);

    public record CreateProjectSucceeded(CreateProjectResponseDto CreateProjectResponseDto);
    public record CreateProjectFailed;

    public record UpdateProjectSucceeded(UpdateProjectResponseDto UpdateProjectResponseDto);
    public record UpdateProjectFailed;

    public record DeleteProjectSucceeded;
    public record DeleteProjectFailed;
}
