using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Shared.Domain.Abstractions.Bus;
using Shared.Domain.Abstractions.DateTime;
using Shared.Domain.Abstractions.FileStorage;
using Shared.Domain.Abstractions.Rest;
using Shared.Domain.Abstractions.Security;
using Shared.Infrastructure;
using Shared.Infrastructure.Bus;
using Shared.Infrastructure.Jwt;
using Shared.Infrastructure.Rest;
using Shared.Infrastructure.Storage;

namespace Shared
{
    public static class ServiceRegistration
    {
        public static void AddInfrastructureExtensions(this IServiceCollection services)
        {
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped<IDateTimeService, DateTimeService>();
            services.AddScoped<IAuthenticatedUserService, AuthenticatedUserService>();
            services.AddScoped<IStorageBlobService, StorageBlobService>();
            services.AddScoped<IStorageFileShareService, StorageFileShareService>();
            services.AddScoped<IRestService, RestService>();
            services.AddScoped<IEventHandler, InMemoryBus>();
            services.AddScoped<ICommandHandler, InMemoryBus>();
            services.AddScoped<IMemoryBus, InMemoryBus>();
            services.AddScoped<IServiceBus, ServiceBus>();
            services.AddScoped<IJwt, Jwt>();
        }
    }
}
