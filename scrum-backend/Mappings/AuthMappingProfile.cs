using AutoMapper;
using scrum_backend.Dtos.Auth.Responses;
using scrum_backend.Models.AppUsers;

namespace scrum_backend.Mappings
{
    public class AuthMappingProfile : Profile
    {
        public AuthMappingProfile()
        {
            CreateMap<AppUser, LoginResponseDto>();
        }
    }
}
