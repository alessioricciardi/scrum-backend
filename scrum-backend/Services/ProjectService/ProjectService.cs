using OneOf;
using scrum_backend.Data;
using scrum_backend.Models.AppUsers;
using scrum_backend.Models.Projects;
using scrum_backend.Results;
using scrum_backend.Dtos.Project.Responses;
using scrum_backend.Dtos.Project.Requests;
using AutoMapper;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using scrum_backend.Authorization.Policies;


namespace scrum_backend.Services.ProjectService
{
    public class ProjectService : IProjectService
    {
        private readonly AppDbContext _appDbContext;
        private readonly IMapper _mapper;
        private readonly IAuthorizationService _authorizationService;
        private readonly ILogger<ProjectService> _logger;

        public ProjectService(AppDbContext appDbContext, IAuthorizationService authorizationService, IMapper mapper, ILogger<ProjectService> logger)
        {
            _appDbContext = appDbContext;
            _authorizationService = authorizationService;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<OneOf<ProjectGetSucceeded, ProjectNotFound>> GetProjectByIdAsync(int projectId)
        {
            var project = await _appDbContext.Projects.FindAsync(projectId);
            if (project is null) return new ProjectNotFound();

            var response = _mapper.Map<GetProjectResponseDto>(project);
            return new ProjectGetSucceeded(response);
        }

        public async Task<OneOf<ProjectCreateSucceeded, ProjectCreateFailed, UserNotFound, Unauthorized>> CreateProjectAsync(CreateProjectRequestDto createProjectDto, ClaimsPrincipal user)
        {
            var userId = user.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId is null)
                return new Unauthorized();

            var appUser = await _appDbContext.FindAsync<AppUser>(userId);
            if (appUser is null)
                return new UserNotFound();

            var project = new Project
            {
                Name = createProjectDto.Name,
                Description = createProjectDto.Description,
                OwnerId = userId,
            };

            _appDbContext.Projects.Add(project);

            try
            {
                await _appDbContext.SaveChangesAsync();

                var response = _mapper.Map<CreateProjectResponseDto>(project);
                return new ProjectCreateSucceeded(response);
            }
            catch
            {
                return new ProjectCreateFailed();
            }
        }

        public async Task<OneOf<ProjectUpdateSucceeded, ProjectUpdateFailed, ProjectNotFound, Forbidden>> UpdateProjectAsync(int projectId, UpdateProjectRequestDto updateProjectDto, ClaimsPrincipal user)
        {
            var project = await _appDbContext.Projects.FindAsync(projectId);
            if (project is null)
                return new ProjectNotFound();

            var authResult = await _authorizationService.AuthorizeAsync(
                user, project, nameof(AuthorizationPolicies.ProjectOwner));

            if (!authResult.Succeeded)
                return new Forbidden();

            if (!string.IsNullOrEmpty(updateProjectDto.Name))
                project.Name = updateProjectDto.Name;

            if (!string.IsNullOrEmpty(updateProjectDto.Description))
                project.Description = updateProjectDto.Description;


            _appDbContext.Projects.Update(project);

            try
            {
                await _appDbContext.SaveChangesAsync();

                var response = _mapper.Map<UpdateProjectResponseDto>(project);
                return new ProjectUpdateSucceeded(response);
            }
            catch
            {
                return new ProjectUpdateFailed();
            }

        }

        public async Task<OneOf<ProjectDeleteSucceeded, ProjectDeleteFailed, ProjectNotFound, Forbidden>> DeleteProjectAsync(int projectId, ClaimsPrincipal user)
        {
            var project = await _appDbContext.Projects.FindAsync(projectId);
            if (project is null)
                return new ProjectNotFound();

            var authResult = await _authorizationService.AuthorizeAsync(user, project, nameof(AuthorizationPolicies.ProjectOwner));
            if (!authResult.Succeeded)
                return new Forbidden();

            _appDbContext.Projects.Remove(project);

            try
            {
                await _appDbContext.SaveChangesAsync();
                return new ProjectDeleteSucceeded();
            }
            catch
            {
                return new ProjectDeleteFailed();
            }

        }
    }
}
