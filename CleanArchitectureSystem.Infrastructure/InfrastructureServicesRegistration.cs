using CleanArchitectureSystem.Application.Contracts.Email;
using CleanArchitectureSystem.Application.Contracts.Logging;
using CleanArchitectureSystem.Application.Models.Email;
using CleanArchitectureSystem.Infrastructure.EmailService;
using CleanArchitectureSystem.Infrastructure.Logging;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CleanArchitectureSystem.Infrastructure
{
    public static class InfrastructureServicesRegistration
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services,
            IConfiguration configuration)
        {
            services.Configure<EmailSettings>(configuration.GetSection("EmailSettings"));
            services.AddTransient<IEmailSender, EmailSender>();
            services.AddScoped(typeof(IAppLogger<>), typeof(LoggerAdapter<>));
            return services;
        }
    }
}
