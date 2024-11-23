using AutoMapper;
using MyndMapper.Entities;

namespace MyndMapper.DTOs.UserDTOs;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<Canvas, int>().ConvertUsing(src => src.Id);
        CreateMap<User, UserGetDto>().ForMember(dest => dest.CreatedCanvases, opt => opt.MapFrom(src => src.CreatedCanvases));
        CreateMap<UserPostDto, User>();
        CreateMap<UserPutDto, User>();
    }
}