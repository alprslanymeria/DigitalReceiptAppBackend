using App.Application.Contracts.Persistence;
using App.Persistence;

namespace App.API.Extensions;

public static class PersistenceExtension
{
    public static IServiceCollection AddRepositories(this IServiceCollection services, IConfiguration configuration)
    {
        // CONNECTION STRING
        var connString = configuration.GetConnectionString("SqlServer");

        // DB CONTEXT
        services.AddDbContext<AppDbContext>(options =>
        {
            DbContextConfigurator.Configure(options, connString!, typeof(PersistenceAssembly).Assembly.FullName!);
        });

        // UNIT OF WORK
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        // GENERIC REPOSITORY (KEPT FOR BACKWARD COMPATIBILITY)
        services.AddScoped(typeof(IGenericRepository<,>), typeof(GenericRepository<,>));

        // ENTITY-SPECIFIC REPOSITORIES

        return services;
    }
}
