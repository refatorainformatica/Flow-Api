using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Services.Features.Peoples.ActivityBranchs.Repositories;
using Shared.Domain.Abstractions.Modules;

namespace Services.Features.Peoples.ActivityBranchs
{
    public class ActivityBranchsModule : IModule
    {
        public static void ConfigureServices(
            IServiceCollection services,
            IConfiguration configuration
        )
        {
            services.AddDbContext<ActivityBranchDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("FlowDatabase"))
            );
        }
    }
}
