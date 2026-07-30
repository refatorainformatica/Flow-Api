using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Shared.Domain.Abstractions.Modules
{
    public interface IModule
    {
        static abstract void ConfigureServices(
            IServiceCollection services,
            IConfiguration configuration
        );
    }
}
