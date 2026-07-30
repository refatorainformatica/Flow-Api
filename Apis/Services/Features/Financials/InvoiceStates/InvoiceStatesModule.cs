using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Services.Features.Financials.InvoiceStates.Repositories;
using Shared.Domain.Abstractions.Modules;

namespace Services.Features.Financials.InvoiceStates
{
    public class InvoiceStatesModule : IModule
    {
        public static void ConfigureServices(
            IServiceCollection services,
            IConfiguration configuration
        )
        {
            services.AddDbContext<InvoiceStateDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("FlowDatabase"))
            );
        }
    }
}
