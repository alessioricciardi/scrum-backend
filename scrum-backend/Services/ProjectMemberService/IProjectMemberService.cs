using OneOf;
using scrum_backend.Dtos.ProjectMember.Requests;
using scrum_backend.Models.Projects;
using scrum_backend.Results;
using System.Security.Claims;

namespace scrum_backend.Services.ProjectMemberService
{
    public interface IProjectMemberService
    {
        Task<OneOf<MembersGetSucceeded, ProjectNotFound, Forbidden>> GetMembersAsync(int projectId, ClaimsPrincipal user);
        Task<OneOf<MemberGetByIdSucceeded, ProjectNotFound, MemberNotFound, Forbidden>> GetMemberByIdAsync(int projectId, int memberId,  ClaimsPrincipal user);
        Task<OneOf<MemberAddSucceeded, MemberAddFailed, UserNotFound, UserIsAlreadyAMember, ProjectNotFound, InvalidRole, Forbidden>> AddMemberAsync(int projectId, AddProjectMemberRequestDto addProjectMemberRequestDto, ClaimsPrincipal user);
        Task<OneOf<MemberRoleUpdateSucceeded, MemberRoleUpdateFailed, ProjectNotFound, MemberNotFound, InvalidRole, Forbidden>> UpdateMemberRoleAsync(int projectId, int memberId, UpdateProjectMemberRoleRequestDto updateProjectMemberRoleRequestDto, ClaimsPrincipal user);
        Task<OneOf<MemberRemoveSucceeded, MemberRemoveFailed, MemberNotFound, ProjectNotFound, Forbidden>> RemoveMemberAsync(int projectId, int memberId, ClaimsPrincipal user);
    }
}
