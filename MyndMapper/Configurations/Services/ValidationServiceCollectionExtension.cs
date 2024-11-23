using FluentValidation;
using MyndMapper.DTOs.CanvasDtos;
using MyndMapper.DTOs.UserDTOs;
using MyndMapper.Validators.CanvasDTOs;

namespace MyndMapper.Configurations.Services;

public static class ValidationServiceCollectionExtension
{
    public static IServiceCollection AddValidators(this IServiceCollection services)
    {
        services.AddScoped<IValidator<UserPostDto>, UserPostDtoValidator>();
        services.AddScoped<IValidator<UserPutDto>, UserPutDtoValidator>();
        services.AddScoped<IValidator<CanvasPostDto>, CanvasPostDtoValidator>();
        services.AddScoped<IValidator<CanvasPutDto>, CanvasPutDtoValidator>();
        return services;
    }
}