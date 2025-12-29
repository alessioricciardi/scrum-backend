using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using OneOf;
using scrum_backend.Authorization.Policies;
using scrum_backend.Data;
using scrum_backend.Dtos.ProjectMember.Requests;
using scrum_backend.Dtos.ProjectMember.Responses;
using scrum_backend.Models.AppUsers;
using scrum_backend.Models.Projects;
using scrum_backend.Results;
using System.Security.Claims;

namespace scrum_backend.Services.ProjectMemberService
{
    public class ProjectMemberService : IProjectMemberService
    {
        private readonly AppDbContext _appDbContext;
        private readonly IMapper _mapper;
        private readonly IAuthorizationService _authorizationService;
        private readonly ILogger<ProjectMemberService> _logger;

        public ProjectMemberService(AppDbContext appDbContext, IMapper mapper, IAuthorizationService authorizationService, ILogger<ProjectMemberService> logger)
        {
            _appDbContext = appDbContext;
            _mapper = mapper;
            _authorizationService = authorizationService;
            _logger = logger;
        }

        private async Task<OneOf<AuthorizationSucceeded, ProjectNotFound, Forbidden>> CheckIfAuthorizedAsync(int projectId, ClaimsPrincipal user)
        {
            var project = await _appDbContext.Projects
                                    .FirstOrDefaultAsync(p => p.Id == projectId);

            if (project == null)
                return new ProjectNotFound();

            var authResult = await _authorizationService.AuthorizeAsync(user, project, AuthorizationPolicies.ProjectOwner);
            if (!authResult.Succeeded)
                return new Forbidden();

            return new AuthorizationSucceeded();
        }

        public async Task<OneOf<MembersGetSucceeded, ProjectNotFound, Forbidden>> GetMembersAsync(int projectId, ClaimsPrincipal user)
        {
            var authResult = await CheckIfAuthorizedAsync(projectId, user);
            if (authResult.IsT1) return authResult.AsT1;
            if (authResult.IsT2) return authResult.AsT2;

            var members = await _appDbContext.ProjectMembers
                                .Where(m => m.ProjectId == projectId)
                                .ToListAsync();

            var result = _mapper.Map<IEnumerable<GetProjectMemberResponseDto>>(members);
            return new MembersGetSucceeded(result);
        }

        public async Task<OneOf<MemberGetByIdSucceeded, ProjectNotFound, MemberNotFound, Forbidden>> GetMemberByIdAsync(int projectId, int memberId, ClaimsPrincipal user)
        {
            var authResult = await CheckIfAuthorizedAsync(projectId, user);
            if (authResult.IsT1) return authResult.AsT1;
            if (authResult.IsT2) return authResult.AsT2;

            var member = await _appDbContext.ProjectMembers
                                .FirstOrDefaultAsync(m => m.ProjectId == projectId && m.Id == memberId);

            if (member is null)
                return new MemberNotFound();


            var result = _mapper.Map<GetProjectMemberResponseDto>(member);
            return new MemberGetByIdSucceeded(result);
        }

        public async Task<OneOf<MemberAddSucceeded, MemberAddFailed, UserNotFound, UserIsAlreadyAMember, ProjectNotFound, InvalidRole, Forbidden>> AddMemberAsync(int projectId, AddProjectMemberRequestDto addProjectMemberRequestDto, ClaimsPrincipal user)
        {
            var authResult = await CheckIfAuthorizedAsync(projectId, user);
            if (authResult.IsT1) return authResult.AsT1;
            if (authResult.IsT2) return authResult.AsT2;

            var userId = addProjectMemberRequestDto.UserId;
            var role = addProjectMemberRequestDto.Role;

            if (!Enum.IsDefined(typeof(ProjectMemberRole), role))
                return new InvalidRole();

            var appUser = await _appDbContext.FindAsync<AppUser>(userId);
            if (appUser is null)
                return new UserNotFound();

            var alreadyExists = await _appDbContext.ProjectMembers
                                        .AnyAsync(m => m.ProjectId == projectId && m.UserId == userId);
            
            if (alreadyExists)
                return new UserIsAlreadyAMember();

            var projectMember = new ProjectMember
            {
                UserId = userId,
                ProjectId = projectId,
                Role = role
            };

            try
            {
                _appDbContext.ProjectMembers.Add(projectMember);
                await _appDbContext.SaveChangesAsync();

                var response = _mapper.Map<AddProjectMemberResponseDto>(projectMember);
                return new MemberAddSucceeded(response);
            }
            catch
            {
                return new MemberAddFailed();
            }
        }
        public async Task<OneOf<MemberRoleUpdateSucceeded, MemberRoleUpdateFailed, ProjectNotFound, MemberNotFound, InvalidRole, Forbidden>> UpdateMemberRoleAsync(int projectId, int memberId, UpdateProjectMemberRoleRequestDto updateProjectMemberRoleRequestDto, ClaimsPrincipal user)
        {
            var authResult = await CheckIfAuthorizedAsync(projectId, user);
            if (authResult.IsT1) return authResult.AsT1;
            if (authResult.IsT2) return authResult.AsT2;

            var role = updateProjectMemberRoleRequestDto.Role;

            if (!Enum.IsDefined(typeof(ProjectMemberRole), role))
                return new InvalidRole();

            var member = await _appDbContext.ProjectMembers
                                .FirstOrDefaultAsync(m => m.Id == memberId && m.ProjectId == projectId);

            if (member is null)
                return new MemberNotFound();

            try
            {
                member.Role = role;

                await _appDbContext.SaveChangesAsync();
                return new MemberRoleUpdateSucceeded();
            }
            catch
            {
                return new MemberRoleUpdateFailed();
            }
        }


        public async Task<OneOf<MemberRemoveSucceeded, MemberRemoveFailed, MemberNotFound, ProjectNotFound, Forbidden>> RemoveMemberAsync(int projectId, int memberId, ClaimsPrincipal user)
        {
            var authResult = await CheckIfAuthorizedAsync(projectId, user);
            if (authResult.IsT1) return authResult.AsT1;
            if (authResult.IsT2) return authResult.AsT2;

            var member = await _appDbContext.ProjectMembers
                            .FirstOrDefaultAsync(m => m.Id == memberId && m.ProjectId == projectId);

            if (member == null)
                return new MemberNotFound();

            try
            {
                _appDbContext.ProjectMembers.Remove(member);
                await _appDbContext.SaveChangesAsync();
                return new MemberRemoveSucceeded();
            }
            catch
            {
                return new MemberRemoveFailed();
            }
        }


    }
}
