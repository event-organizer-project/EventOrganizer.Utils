using EventOrganizer.Utils.Logging;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Security.Cryptography.X509Certificates;

namespace EventOrganizer.Utils.WebApplicationExtensions
{
    public static class WebApplicationExtensions
    {
        public static void ConfigureCertificates(this WebApplicationBuilder builder)
        {
            _ = bool.TryParse(builder.Configuration["UseCustomSslCertificates"], out var useCustomSslCertificates);

            if (useCustomSslCertificates)
            {
                var certs = new Dictionary<string, X509Certificate2>(StringComparer.OrdinalIgnoreCase)
                {
                    ["localhost"] = new X509Certificate2("/app/certificates/localhost.pfx", "password"),
                    ["host.docker.internal"] = new X509Certificate2("/app/certificates/host.docker.internal.pfx", "password")
                };

                using (var store = new X509Store(StoreName.Root, StoreLocation.CurrentUser))
                {
                    store.Open(OpenFlags.ReadWrite);

                    store.Add(certs["localhost"]);
                    store.Add(certs["host.docker.internal"]);
                }

                builder.WebHost.ConfigureKestrel(options =>
                    options.ConfigureHttpsDefaults(opt =>
                    {
                        opt.SslProtocols = System.Security.Authentication.SslProtocols.Tls12;
                        opt.ServerCertificateSelector = (connectionContext, name) =>
                            name == "host.docker.internal" ? certs["host.docker.internal"] : certs["localhost"];
                    }));
            }
        }

        public static void AddCustomLogger(this IServiceCollection services)
        {
            services.AddSingleton<ILoggerProvider, CustomLoggerProvider>();
        }
    }
}
