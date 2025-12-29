using scrum_backend.Dtos.Project.Responses;

namespace scrum_backend.Results
{
    public record ProjectNotFound;
    public record ProjectGetSucceeded(GetProjectResponseDto GetProjectResponseDto);

    public record ProjectCreateSucceeded(CreateProjectResponseDto CreateProjectResponseDto);
    public record ProjectCreateFailed;

    public record ProjectUpdateSucceeded(UpdateProjectResponseDto UpdateProjectResponseDto);
    public record ProjectUpdateFailed;

    public record ProjectDeleteSucceeded;
    public record ProjectDeleteFailed;
}
