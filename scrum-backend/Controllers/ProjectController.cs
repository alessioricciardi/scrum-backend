using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using scrum_backend.Dtos;
using scrum_backend.Dtos.Project.Requests;
using scrum_backend.Results;
using scrum_backend.Services.ProjectService;

namespace scrum_backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProjectController : ControllerBase
    {
        private readonly IProjectService _projectService;

        public ProjectController(IProjectService projectService)
        {
            _projectService = projectService;
        }

        [Authorize]
        [HttpGet("{projectId:int}")]
        public async Task<IActionResult> GetProjectById(int projectId)
        {
            var result = await _projectService.GetProjectByIdAsync(projectId);

            return result.Match<IActionResult>(
                    projectGetSucceeded => Ok(projectGetSucceeded.GetProjectResponseDto),
                    projectNotFound => NotFound(new ErrorResponseDto{Message = "Project with given id was not found."})
            );
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreateProject([FromBody] CreateProjectRequestDto createProjectDto)
        {
            var result = await _projectService.CreateProjectAsync(createProjectDto, User);

            return result.Match<IActionResult>(
                    createdProject =>
                    {
                        var dto = createdProject.CreateProjectResponseDto;
                        return CreatedAtAction(nameof(GetProjectById), new { projectId = dto.Id }, dto);
                    },
                    projectCreateFailed => StatusCode(500, new ErrorResponseDto{Message = "Failed to create project." }),
                    userNotFound => Unauthorized(),
                    unauthorized => Unauthorized()
                );
        }

        [Authorize]
        [HttpPut("{projectId:int}")]
        public async Task<IActionResult> UpdateProject(int projectId, [FromBody] UpdateProjectRequestDto updateProjectDto)
        {
            var result = await _projectService.UpdateProjectAsync(projectId, updateProjectDto, User);

            return result.Match<IActionResult>(
                projectUpdateSucceeded => Ok(projectUpdateSucceeded.UpdateProjectResponseDto),
                projectUpdateFailed => StatusCode(500, new ErrorResponseDto{ Message = "Failed to update project." }),
                projectNotFound =>NotFound(new ErrorResponseDto{Message = "Failed to update project with given id." }),
                forbidden => Forbid()
                );
        }

        [Authorize]
        [HttpDelete("{projectId:int}")]
        public async Task<IActionResult> DeleteProject(int projectId)
        {
            var result = await _projectService.DeleteProjectAsync(projectId, User);

            return result.Match<IActionResult>(
                projectDeleteSucceeded => NoContent(),
                projectDeleteFailed => StatusCode(500, new ErrorResponseDto{Message = "Failed to delete project."}),
                projectNotFound => NotFound(new ErrorResponseDto{Message = "Failed to delete project with given id."}),
                forbidden => Forbid()
                );
        }
    }
}
