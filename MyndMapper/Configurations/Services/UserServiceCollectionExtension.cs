using MyndMapper.Storages.UserStorage;

namespace Microsoft.Extensions.DependencyInjection;

public static class UserServiceCollectionExtension
{
    public static IServiceCollection AddUser(this IServiceCollection services)
    {
        services.AddSingleton<IUserStorage, UserStorage>();

        return services;
    }
}