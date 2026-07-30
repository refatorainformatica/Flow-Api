using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using Shared.Domain.Abstractions.Modules;

namespace Application.Configuration
{
    [ExcludeFromCodeCoverage]
    /// <summary>
    /// Provides methods for discovering and configuring modules in the application.
    /// </summary>
    public static class ModuleDiscovery
    {
        private static readonly Type ModuleType = typeof(IModule);

        /// <summary>
        /// Configures the modules in the application by discovering and invoking their configuration methods.
        /// </summary>
        /// <param name="services">The service collection to which the modules will be added.</param>
        /// <param name="assemblies">The assemblies to scan for module types.</param>
        /// <param name="configuration">The application configuration to pass to the modules.</param>
        public static void ConfigureModules(
            this IServiceCollection services,
            Assembly[] assemblies,
            IConfiguration configuration
        )
        {
            if (assemblies.Length == 0)
            {
                throw new ArgumentException(
                    "At least one assembly must be provided.",
                    nameof(assemblies)
                );
            }

            foreach (var type in GetModuleTypes(assemblies))
            {
                var method = GetMapEndpointMethod(type);
                method?.Invoke(null, [services, configuration]);
            }
        }

        private static IEnumerable<Type> GetModuleTypes(params Assembly[] assemblies) =>
            assemblies
                .SelectMany(x => x.GetTypes())
                .Where(x =>
                    ModuleType.IsAssignableFrom(x) && x is { IsInterface: false, IsAbstract: false }
                );

        private static MethodInfo GetMapEndpointMethod(IReflect type) =>
            type.GetMethod(
                nameof(IModule.ConfigureServices),
                BindingFlags.Static | BindingFlags.Public
            );
    }
}
