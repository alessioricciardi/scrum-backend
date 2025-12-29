using AutoMapper;
using scrum_backend.Dtos.Project.Responses;
using scrum_backend.Models.Projects;

namespace scrum_backend.Mappings
{
    public class ProjectMappingProfile : Profile
    {
        public ProjectMappingProfile()
        {
            CreateMap<Project, GetProjectResponseDto>();
            CreateMap<Project, CreateProjectResponseDto>();
            CreateMap<Project, UpdateProjectResponseDto>();
        }
    }
}
