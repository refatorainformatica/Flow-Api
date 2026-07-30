namespace Application.Configuration.Extensions
{
    /// <summary>
    /// Provides extension methods for configuring HttpClient services.
    /// </summary>
    public static class HttpClientExtension
    {
        /// <summary>
        /// Configures a custom HttpClient with specific settings and handlers.
        /// </summary>
        /// <param name="services">The IServiceCollection to add the HttpClient to.</param>
        public static void ConfigureCustomHttpClient(this IServiceCollection services)
        {
            services
                .AddHttpClient(
                    "HttpClient",
                    client =>
                    {
                        client.Timeout = TimeSpan.FromSeconds(10);
                    }
                )
                .ConfigurePrimaryHttpMessageHandler(
                    () =>
                        new HttpClientHandler
                        {
                            ClientCertificateOptions = ClientCertificateOption.Manual,
                            ServerCertificateCustomValidationCallback = (
                                httpRequestMessage,
                                cert,
                                cetChain,
                                policyErrors
                            ) =>
                            {
                                // Implement proper validation logic here
                                return policyErrors == System.Net.Security.SslPolicyErrors.None;
                            },
                        }
                );
        }
    }
}
