using scrum_backend.Dtos.ProjectMember.Responses;

namespace scrum_backend.Results
{
    public record GetMembersSucceeded(IEnumerable<GetProjectMemberResponseDto> GetProjectMembersResponseDto);
    public record GetMemberByIdSucceeded(GetProjectMemberResponseDto GetProjectMemberResponseDto);

    public record AddMemberSucceeded(AddProjectMemberResponseDto AddProjectMemberResponseDto);
    public record AddMemberFailed;

    public record UpdateMemberRoleSucceeded;
    public record UpdateMemberRoleFailed;

    public record RemoveMemberSucceeded;
    public record RemoveMemberFailed;

    public record MemberNotFound;
    public record UserIsAlreadyAMember;
}
