using CleanArchitectureSystem.Application.Contracts.Interface;
using CleanArchitectureSystem.Persistence.DatabaseContext;
using CleanArchitectureSystem.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CleanArchitectureSystem.Persistence
{
    public static class PersistenceServiceRegistration
    {
        public static IServiceCollection AddPersistenceServices(this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddDbContext<CleanArchDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("CQRS_CleanArchConnectionString"));
            });

            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddScoped<IBatchSerialRepository, BatchSerialRepository>();
            services.AddScoped<IMainSerialRepository, MainSerialRepository>();


            return services;
        }

    }
}
