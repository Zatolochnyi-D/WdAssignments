using MyndMapper.Storages.CanvasStorage;

namespace MyndMapper.Configurations.Services;

public static class CanvasServiceCollectionExtension
{
    public static IServiceCollection AddCanvas(this IServiceCollection services)
    {
        services.AddSingleton<ICanvasStorage, CanvasStorage>();

        return services;
    }
}