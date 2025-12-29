using scrum_backend.Dtos.ProjectMember.Responses;

namespace scrum_backend.Results
{
    public record MembersGetSucceeded(IEnumerable<GetProjectMemberResponseDto> GetProjectMembersResponseDto);
    public record MemberGetByIdSucceeded(GetProjectMemberResponseDto GetProjectMemberResponseDto);

    public record MemberAddSucceeded(AddProjectMemberResponseDto AddProjectMemberResponseDto);
    public record MemberAddFailed;

    public record MemberRoleUpdateSucceeded;
    public record MemberRoleUpdateFailed;

    public record MemberRemoveSucceeded;
    public record MemberRemoveFailed;

    public record MemberNotFound;
    public record UserIsAlreadyAMember;
}
