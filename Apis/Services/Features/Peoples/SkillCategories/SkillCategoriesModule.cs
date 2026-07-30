using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Services.Features.Peoples.SkillCategories.Repositories;
using Shared.Domain.Abstractions.Modules;

namespace Services.Features.Peoples.SkillCategories
{
    public class SkillCategoriesModule : IModule
    {
        public static void ConfigureServices(
            IServiceCollection services,
            IConfiguration configuration
        )
        {
            services.AddDbContext<SkillCategoryDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("FlowDatabase"))
            );
        }
    }
}
