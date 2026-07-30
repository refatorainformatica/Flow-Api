using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Services.Features.Peoples.Sponsors.Repositories;
using Shared.Domain.Abstractions.Modules;

namespace Services.Features.Peoples.Sponsors
{
    public class SponsorsModule : IModule
    {
        public static void ConfigureServices(
            IServiceCollection services,
            IConfiguration configuration
        )
        {
            services.AddDbContext<SponsorDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("FlowDatabase"))
            );
        }
    }
}
