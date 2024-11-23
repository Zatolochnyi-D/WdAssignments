using AutoMapper;
using MyndMapper.Entities;

namespace MyndMapper.DTOs.CanvasDtos;

public class CanvasProfile : Profile
{
    public CanvasProfile()
    {
        CreateMap<Canvas, CanvasGetDto>();
        CreateMap<CanvasPostDto, Canvas>();
    }
}