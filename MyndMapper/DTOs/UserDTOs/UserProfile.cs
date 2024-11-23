using AutoMapper;
using MyndMapper.Entities;

namespace MyndMapper.DTOs.UserDTOs;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<User, UserGetDto>();
        CreateMap<UserPostDto, User>();
        CreateMap<UserPutDto, User>();
    }
}