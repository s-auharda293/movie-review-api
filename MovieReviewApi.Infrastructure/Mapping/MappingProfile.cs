using AutoMapper;
using MovieReviewApi.Application.DTOs;
using MovieReviewApi.Domain.Entities;

namespace MovieReviewApi.Infrastructure.Mapping
{
    public class MappingProfile:Profile
    {
        public MappingProfile()
        {
            CreateMap<ApplicationUser, UserResponse>();
            CreateMap<ApplicationUser, CurrentUserResponse>();
            CreateMap<UserRegisterRequest, ApplicationUser>(); //when registering a user
        }
    }
}
