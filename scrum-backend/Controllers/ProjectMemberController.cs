using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using scrum_backend.Authorization.Policies;
using scrum_backend.Dtos;
using scrum_backend.Dtos.ProjectMember.Requests;
using scrum_backend.Results;
using scrum_backend.Services.ProjectMemberService;

namespace scrum_backend.Controllers
{
    [ApiController]
    [Route("api/projects/{projectId:int}/members")]
    public class ProjectMemberController : ControllerBase
    {
        private readonly IProjectMemberService _projectMemberService;

        public ProjectMemberController(IProjectMemberService projectMemberService)
        {
            _projectMemberService = projectMemberService;
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetProjectMembers([FromRoute] int projectId)
        {
            var result = await _projectMemberService.GetMembersAsync(projectId, User);

            return result.Match<IActionResult>(
                getMembersSucceeded => Ok(getMembersSucceeded.GetProjectMembersResponseDto),
                projectNotFound => NotFound(new ErrorResponseDto { Message = "Project not found." }),
                forbidden => Forbid()
            );
        }

        [Authorize]
        [HttpGet("{memberId:int}")]
        public async Task<IActionResult> GetProjectMemberById([FromRoute] int projectId, int memberId)
        {
            var result = await _projectMemberService.GetMemberByIdAsync(projectId, memberId, User);

            return result.Match<IActionResult>(
                getMemberByIdSucceeded => Ok(getMemberByIdSucceeded.GetProjectMemberResponseDto),
                memberNotFound => NotFound(new ErrorResponseDto { Message = "Member not found." }),
                projectNotFound => NotFound(new ErrorResponseDto { Message = "Project not found." }),
                forbidden => Forbid()
            );
        }


        [Authorize]
        [HttpPost]
        public async Task<IActionResult> AddProjectMember([FromRoute] int projectId, [FromBody] AddProjectMemberRequestDto addProjectMemberRequestDto)
        {
            var result = await _projectMemberService.AddMemberAsync(projectId, addProjectMemberRequestDto, User);

            return result.Match<IActionResult>(
                addMemberSucceeded =>
                {
                    var dto = addMemberSucceeded.AddProjectMemberResponseDto;
                    return CreatedAtAction(
                        nameof(GetProjectMemberById),
                        new { projectId, memberId = dto.Id },
                        dto);
                },
                addMemberFailed => StatusCode(500, new ErrorResponseDto { Message = "Failed to add a member." }),
                userNotFound => NotFound(new ErrorResponseDto { Message = "User not found." }),
                userIsAlreadyMember => Conflict(new ErrorResponseDto { Message = "User is already a member." }),
                projectNotFound => NotFound(new ErrorResponseDto { Message = "Project not found." }),
                invalidRole => BadRequest(new ErrorResponseDto { Message = "Invalid project role." }),
                forbidden => Forbid()
            );
        }

        [Authorize]
        [HttpPatch("{memberId:int}/Role")]
        public async Task<IActionResult> UpdateProjectMemberRole ([FromRoute] int projectId, int memberId, [FromBody] UpdateProjectMemberRoleRequestDto updateProjectMemberRoleRequestDto)
        {
            var result = await _projectMemberService.UpdateMemberRoleAsync(projectId, memberId, updateProjectMemberRoleRequestDto, User);

            return result.Match<IActionResult>(
                updatMemberRoleSucceeded => NoContent(),
                updateMemberRoleFailed => StatusCode(500, new ErrorResponseDto { Message = "Failed to update a member role." }),
                memberNotFound => NotFound(new ErrorResponseDto { Message = "Member not found." }),
                projectNotFound => NotFound(new ErrorResponseDto { Message = "Project not found." }),
                invalidRole => BadRequest(new ErrorResponseDto { Message = "Invalid project role." }),
                forbidden => Forbid()
            );
        }


        [Authorize]
        [HttpDelete("{memberId:int}")]
        public async Task<IActionResult> RemoveProjectMember([FromRoute] int projectId, int memberId)
        {
            var result = await _projectMemberService.RemoveMemberAsync(projectId, memberId, User);

            return result.Match<IActionResult>(
                removeMemberSucceeded => NoContent(),
                removeMemberFailed => StatusCode(500, new ErrorResponseDto { Message = "Failed to remove a member." }),
                memberNotFound => NotFound(new ErrorResponseDto { Message = "Member not found." }),
                projectNotFound => NotFound(new ErrorResponseDto { Message = "Project not found." }),
                forbidden => Forbid()
            );
        }


    }
}
