using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Services.Features.Peoples.ProfessionalProfiles.Repositories;
using Shared.Domain.Abstractions.Modules;

namespace Services.Features.Peoples.ProfessionalProfiles
{
    public class ProfessionalProfilesModule : IModule
    {
        public static void ConfigureServices(
            IServiceCollection services,
            IConfiguration configuration
        )
        {
            services.AddDbContext<ProfessionalProfileDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("FlowDatabase"))
            );
        }
    }
}
