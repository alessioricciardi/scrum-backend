using AutoMapper;
using scrum_backend.Dtos.ProjectProductBacklog.Requests;
using scrum_backend.Dtos.ProjectProductBacklog.Responses;
using scrum_backend.Models.Projects;

namespace scrum_backend.Mappings
{
    public class ProjectProductBacklogMappingProfile : Profile
    {
        public ProjectProductBacklogMappingProfile()
        {
            CreateMap<ProductBacklogItem, GetProductBacklogItemResponseDto>();
            CreateMap<CreateProductBacklogItemRequestDto, ProductBacklogItem>();
            CreateMap<UpdateProductBacklogItemRequestDto, ProductBacklogItem>();
            CreateMap<ProductBacklogItem, UpdateProductBacklogItemRequestDto>();
        }
    }
}
