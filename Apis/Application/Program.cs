using Application.Configuration;
using Application.Configuration.Extensions;
using Microsoft.AspNetCore.ResponseCompression;
using Serilog;
using Services;
using Shared;

internal class Program
{
    private static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        var serviceAssembly = AppDomain.CurrentDomain.Load(Constants.Services);

        builder
            .Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddJsonFile(
                $"appsettings.{builder.Environment.EnvironmentName}.json",
                optional: true,
                reloadOnChange: true
            )
            .AddEnvironmentVariables();

        builder.Services.AddControllers();
        builder.Services.AddOpenApi();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddApiVersioningExtension();
        builder.Services.AddHealthChecks();
        builder.Services.AddAutoMapper(serviceAssembly);
        builder.Services.ConfigureModules([serviceAssembly], builder.Configuration);

        builder.Services.AddSwaggerExtension();
        builder.Services.AddExceptionExtension();
        builder.Services.ConfigureActiveADicrectoryAuthentication(builder.Configuration);
        builder.Services.ConfigureCustomAuthentication(builder.Configuration);
        builder.Services.ConfigureAuthorization(builder.Configuration);
        builder.Services.ConfigureCustomHttpClient();

        builder.Services.AddServiceExtensions();
        builder.Services.AddInfrastructureExtensions();

        builder.Services.AddCors(options =>
            options.AddPolicy("AllowAll", p => p.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader())
        );

        builder.Services.AddResponseCompression(options =>
        {
            options.EnableForHttps = true;
            options.MimeTypes = ["application/json"];
            options.Providers.Add<BrotliCompressionProvider>();
        });

        builder.ConfigureLogger(builder.Configuration);

        var app = builder.Build();

        app.UseRouting();
        app.UseStaticFiles();
        app.UseCors("AllowAll");
        app.UseExceptionHandler();
        app.UseHealthChecks("/health");
        app.UseHttpsRedirection();
        app.UseResponseCompression();
        app.UseSwaggerExtension();
        app.UseSerilogRequestLogging();
        app.UseAuthentication();
        app.UseAuthorization();
        app.MapControllers();

        await app.RunAsync();
    }
}
