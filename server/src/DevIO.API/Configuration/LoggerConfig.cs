using DevIO.API.Extensions;
using Elmah.Io.AspNetCore;
using Elmah.Io.AspNetCore.HealthChecks;
using Elmah.Io.Extensions.Logging;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DevIO.API.Configuration
{
    public static class LoggerConfig
    {
        public static IServiceCollection AddLoggingConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddElmahIo(p =>
            {
                p.ApiKey = "1d70d43c5b4b426791bae0824b012e97";
                p.LogId = new Guid("c08b787c-be75-4410-bf2c-381be2bf0ae7");
            });

            // Conectando Elmah com o Logger do ASP.NET
            services.AddLogging(builder =>
            {
                builder.AddElmahIo(p =>
                {
                    p.ApiKey = "1d70d43c5b4b426791bae0824b012e97";
                    p.LogId = new Guid("c08b787c-be75-4410-bf2c-381be2bf0ae7");
                });

                builder.AddFilter<ElmahIoLoggerProvider>(null, LogLevel.Warning);
            });

            services.AddHealthChecks()
                .AddElmahIoPublisher("1d70d43c5b4b426791bae0824b012e97", new Guid("c08b787c-be75-4410-bf2c-381be2bf0ae7"), "API Providers")
                .AddCheck("Products", new SqlServerHealthCheck(configuration.GetConnectionString("DefaultConnection")))
                .AddSqlServer(configuration.GetConnectionString("DefaultConnection"), name: "BancoSQL");

            services.AddHealthChecksUI();


            return services;
        }

        public static IApplicationBuilder UseLoggingConfiguration(this IApplicationBuilder app)
        {
            app.UseElmahIo();

            app.UseHealthChecks("/hc", new HealthCheckOptions()
            {
                Predicate = _ => true,
                ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
            });

            app.UseHealthChecksUI(opt =>
            {
                opt.UIPath = "/hc-ui";
            });

            return app;
        }
    }
}
