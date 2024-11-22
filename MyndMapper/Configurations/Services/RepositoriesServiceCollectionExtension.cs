using MyndMapper.Repositories;
using MyndMapper.Repositories.Contracts;

namespace MyndMapper.Configurations.Services;

public static class RepositoriesServiceCollectionExtension
{
    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddTransient(typeof(IGenericRepository<,>), typeof(GenericRepository<,>));
        services.AddTransient<IUserRepository, UserRepository>();
        return services;
    }
}