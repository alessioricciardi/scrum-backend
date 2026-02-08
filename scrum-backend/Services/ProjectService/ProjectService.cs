using OneOf;
using scrum_backend.Data;
using scrum_backend.Models.AppUsers;
using scrum_backend.Models.Projects;
using scrum_backend.Results;
using scrum_backend.Dtos.Project.Responses;
using scrum_backend.Dtos.Project.Requests;
using AutoMapper;
using System.Security.Claims;
using scrum_backend.Authorization.Policies;
using scrum_backend.Authorization.Services;


namespace scrum_backend.Services.ProjectService
{
    public class ProjectService : IProjectService
    {
        private readonly AppDbContext _appDbContext;
        private readonly IMapper _mapper;
        private readonly IProjectAccessService _projectAccessService;
        private readonly ILogger<ProjectService> _logger;

        public ProjectService(AppDbContext appDbContext, IProjectAccessService projectAccessService, IMapper mapper, ILogger<ProjectService> logger)
        {
            _appDbContext = appDbContext;
            _projectAccessService = projectAccessService;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<OneOf<GetProjectSucceeded, ProjectNotFound>> GetProjectByIdAsync(int projectId)
        {
            var project = await _appDbContext.Projects.FindAsync(projectId);
            if (project is null) return new ProjectNotFound();

            var response = _mapper.Map<GetProjectResponseDto>(project);
            return new GetProjectSucceeded(response);
        }

        public async Task<OneOf<CreateProjectSucceeded, CreateProjectFailed, UserNotFound, Unauthorized>> CreateProjectAsync(CreateProjectRequestDto createProjectDto, ClaimsPrincipal user)
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
                return new CreateProjectSucceeded(response);
            }
            catch
            {
                return new CreateProjectFailed();
            }
        }

        public async Task<OneOf<UpdateProjectSucceeded, UpdateProjectFailed, ProjectNotFound, Forbidden>> UpdateProjectAsync(int projectId, UpdateProjectRequestDto updateProjectDto, ClaimsPrincipal user)
        {
            var authResult = await _projectAccessService.GetAccessAsync(projectId, user, AuthorizationPolicies.ProjectAdminOnly);
            if (authResult.IsT1) return authResult.AsT1;
            if (authResult.IsT2) return authResult.AsT2;

            var project = authResult.AsT0;

            project.Name = updateProjectDto.Name;
            project.Description = updateProjectDto.Description;

            try
            {
                _appDbContext.Projects.Update(project);
                await _appDbContext.SaveChangesAsync();

                var response = _mapper.Map<UpdateProjectResponseDto>(project);
                return new UpdateProjectSucceeded(response);
            }
            catch
            {
                return new UpdateProjectFailed();
            }

        }

        public async Task<OneOf<DeleteProjectSucceeded, DeleteProjectFailed, ProjectNotFound, Forbidden>> DeleteProjectAsync(int projectId, ClaimsPrincipal user)
        {
            var authResult = await _projectAccessService.GetAccessAsync(projectId, user, AuthorizationPolicies.ProjectAdminOnly);
            if (authResult.IsT1) return authResult.AsT1;
            if (authResult.IsT2) return authResult.AsT2;

            var project = authResult.AsT0;

            _appDbContext.Projects.Remove(project);

            try
            {
                await _appDbContext.SaveChangesAsync();
                return new DeleteProjectSucceeded();
            }
            catch
            {
                return new DeleteProjectFailed();
            }
        }
    }
}
