using AutoMapper;
using scrum_backend.Dtos.ProjectMember.Responses;
using scrum_backend.Models.Projects;

namespace scrum_backend.Mappings
{
    public class ProjectMemberMappingProfile : Profile
    {
        public ProjectMemberMappingProfile()
        {
            CreateMap<ProjectMember, GetProjectMemberResponseDto>();
            CreateMap<ProjectMember, AddProjectMemberResponseDto>();
        }
    }
}
