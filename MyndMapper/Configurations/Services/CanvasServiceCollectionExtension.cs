using MyndMapper.Storages.CanvasStorage;

namespace Microsoft.Extensions.DependencyInjection;

public static class CanvasServiceCollectionExtension
{
    public static IServiceCollection AddCanvas(this IServiceCollection services)
    {
        services.AddSingleton<ICanvasStorage, CanvasStorage>();
        
        return services;
    }
}