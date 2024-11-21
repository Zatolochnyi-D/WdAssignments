using MyndMapper.Storages.UserStorage;

namespace MyndMapper.Configurations.Services;

public static class UserServiceCollectionExtension
{
    public static IServiceCollection AddUser(this IServiceCollection services)
    {
        services.AddSingleton<IUserStorage, UserStorage>();

        return services;
    }
}