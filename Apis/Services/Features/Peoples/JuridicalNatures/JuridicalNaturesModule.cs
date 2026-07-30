using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Services.Features.Peoples.JuridicalNatures.Repositories;
using Shared.Domain.Abstractions.Modules;

namespace Services.Features.Peoples.JuridicalNatures
{
    public class JuridicalNatures : IModule
    {
        public static void ConfigureServices(
            IServiceCollection services,
            IConfiguration configuration
        )
        {
            services.AddDbContext<JuridicalNatureDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("FlowDatabase"))
            );
        }
    }
}
