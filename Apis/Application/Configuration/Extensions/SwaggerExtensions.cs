using System.Reflection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Application.Configuration.Extensions
{
    /// <summary>
    /// Provides extension methods for configuring Swagger and API versioning in an ASP.NET Core application.
    /// </summary>
    public static class SwaggerExtensions
    {
        private const string OpenApiSecurityScheme = "Bearer";

        private const string OpenApiSecuritySchemeDescription =
            "Input your Bearer token in this format - Bearer {your token here} to access this API";

        private const string OpenApiInfoDescription =
            "This API is responsible for overall data distribution and authorization.";

        private const string OpenApiInfoTitle = "Flow Public API";
        private const string OpenApiInfoVersion = "v1";

        private static readonly OpenApiContact DefaultContact = new()
        {
            Name = Shared.Infrastructure.Resources.Config.SwaggerContactName,
            Email = Shared.Infrastructure.Resources.Config.SwaggerContactEmail,
            Url = new Uri(Shared.Infrastructure.Resources.Config.SwaggerContactUri),
        };

        /// <summary>
        /// Adds Swagger services to the service collection with default settings.
        /// </summary>
        /// <param name="services">The service collection to add the Swagger services to.</param>
        public static void AddSwaggerExtension(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                ConfigureSwagger(c);
                ConfigureSecurity(c);
                ConfigureXmlComments(c);
            });
        }

        /// <summary>
        /// Configures Swagger settings.
        /// </summary>
        /// <param name="c">The SwaggerGen options.</param>
        private static void ConfigureSwagger(SwaggerGenOptions c)
        {
            c.UseAllOfToExtendReferenceSchemas();
            c.UseInlineDefinitionsForEnums();
            c.UseAllOfForInheritance();

            var tags = GetTagsForMapping();

            foreach (var tagMapping in tags.Keys)
            {
                c.SwaggerDoc(tagMapping.ToLower().Replace(" ", ""), CreateOpenApiInfo());
            }

            c.DocInclusionPredicate(
                (docName, apiDesc) =>
                    !string.IsNullOrWhiteSpace(apiDesc.GroupName)
                    && apiDesc
                        .GroupName.ToLower()
                        .Replace(" ", "")
                        .Contains(docName.ToLower().Replace(" ", ""))
            );

            c.TagActionsBy(api => new List<string> { api.GroupName });
        }

        /// <summary>
        /// Configures the API security for Swagger.
        /// </summary>
        /// <param name="c">The SwaggerGen options.</param>
        private static void ConfigureSecurity(SwaggerGenOptions c)
        {
            c.AddSecurityDefinition(
                OpenApiSecurityScheme,
                new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = OpenApiSecurityScheme,
                    BearerFormat = "JWT",
                    Description = OpenApiSecuritySchemeDescription,
                }
            );

            c.AddSecurityRequirement(
                new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = OpenApiSecurityScheme,
                            },
                            Scheme = OpenApiSecurityScheme,
                            Name = OpenApiSecurityScheme,
                            In = ParameterLocation.Header,
                        },
                        new List<string>()
                    },
                }
            );
        }

        /// <summary>
        /// Configures XML comments for Swagger.
        /// </summary>
        /// <param name="c">The SwaggerGen options.</param>
        private static void ConfigureXmlComments(SwaggerGenOptions c)
        {
            var projectName = Assembly.GetEntryAssembly()?.GetName().Name;
            if (!string.IsNullOrEmpty(projectName))
            {
                var filePath = Path.Combine(AppContext.BaseDirectory, $"{projectName}.xml");
                c.IncludeXmlComments(filePath);
            }
        }

        /// <summary>
        /// Creates the OpenApiInfo object.
        /// </summary>
        /// <returns>The OpenApiInfo object.</returns>
        private static OpenApiInfo CreateOpenApiInfo()
        {
            return new OpenApiInfo
            {
                Version = OpenApiInfoVersion,
                Title = OpenApiInfoTitle,
                Description = OpenApiInfoDescription,
                Contact = DefaultContact,
            };
        }

        /// <summary>
        /// Adds API versioning services to the service collection.
        /// </summary>
        /// <param name="services">The service collection to add API versioning to.</param>
        public static void AddApiVersioningExtension(this IServiceCollection services)
        {
            services
                .AddApiVersioning(config =>
                {
                    config.DefaultApiVersion = new ApiVersion(1, 0);
                    config.AssumeDefaultVersionWhenUnspecified = true;
                    config.ReportApiVersions = true;
                })
                .AddVersionedApiExplorer(options =>
                {
                    options.GroupNameFormat = "'v'VV"; // Version format (v1, v2, etc.)
                    options.SubstituteApiVersionInUrl = true; // Substitute version in the URL
                });
        }

        /// <summary>
        /// Configures the Swagger middleware for the specified <see cref="WebApplication"/>.
        /// </summary>
        /// <param name="app">The <see cref="WebApplication"/> to configure.</param>
        public static void UseSwaggerExtension(this WebApplication app)
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                var tags = GetTagsForMapping();

                foreach (var tagMappingKey in tags.Keys)
                {
                    c.SwaggerEndpoint(
                        $"/swagger/{tagMappingKey.ToLower().Replace(" ", "")}/swagger.json",
                        tags[tagMappingKey] // Displays the tag name in the dropdown
                    );
                }
            });
        }

        /// <summary>
        /// Generates a dictionary that maps API group names to their corresponding tags.
        /// </summary>
        /// <returns>A dictionary where the key is the group name and the value is the tag.</returns>
        private static Dictionary<string, string> GetTagsForMapping()
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            var controllerTypes = assemblies
                .SelectMany(a => a.GetTypes())
                .Where(t =>
                    typeof(ControllerBase).IsAssignableFrom(t)
                    && t.GetCustomAttributes(typeof(ApiExplorerSettingsAttribute), true).Any()
                );

            var tagsForMapping = controllerTypes
                .ToLookup(
                    t => t.GetCustomAttribute<ApiExplorerSettingsAttribute>()?.GroupName,
                    t =>
                        t.GetCustomAttribute<TagsAttribute>()?.Tags?.FirstOrDefault()
                        ?? t.Name.Replace("Controller", "")
                )
                .ToDictionary(g => g.Key, g => g.First()); // Handle duplicates as needed

            return tagsForMapping;
        }
    }
}
