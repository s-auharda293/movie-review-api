using AutoMapper;
using DotnetSecurity.Domain.Contracts;

namespace DotnetSecurity.Infrastructure.Mapping
{
    public class MappingProfile:Profile
    {
        public MappingProfile()
        {
            CreateMap<ApplicationUser, UserResponse>();

            CreateMap<ApplicationUser, CurrentUserResponse>();

            CreateMap<UserRegisterRequest, ApplicationUser>();

        }
    }
}
