using AutoMapper;
using MyndMapper.Entities;

namespace MyndMapper.DTOs.CanvasDtos;

public class CanvasProfile : Profile
{
    public CanvasProfile()
    {
        CreateMap<Canvas, CanvasGetDto>().ForMember(dest => dest.OwnerId, opt => opt.MapFrom(src => src.Owner.Id));
        CreateMap<CanvasPostDto, Canvas>().ForMember(dest => dest.Owner, opt => opt.Ignore());
        CreateMap<CanvasPutDto, Canvas>();
    }
}