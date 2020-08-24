using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Net.Http.Headers;

namespace DevIO.Configuration
{
    public static class ApiConfig
    {
        public static IServiceCollection WebApiConfig (this IServiceCollection services)
        {
            services.AddControllers();

            services.AddApiVersioning(opt =>
            {
                // Quando não tiver v1, v2 etc vai assumir a default
                opt.AssumeDefaultVersionWhenUnspecified = true;
                opt.DefaultApiVersion = new ApiVersion(1, 0);
                opt.ReportApiVersions = true; // vai passar no header da response se a api esta na versao ok ou obsoleta
            });

            services.AddVersionedApiExplorer(opt =>
            {
                opt.GroupNameFormat = "'v'VVV";
                opt.SubstituteApiVersionInUrl = true;
            });

            // Desabilitando configuracao de ModelState
            services.Configure<ApiBehaviorOptions>(
                // Suprimindo a forma de validacao automatica 
                options => options.SuppressModelStateInvalidFilter = true
            );
            
            // CORS sao utilizados para "relaxar" a segurança da aplicação

            // habilitando cors para o front consumir a api
            services.AddCors(opt => {
                opt.AddPolicy("Development",
                builder => builder
                    .AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader());
            });

            services.AddCors(opt => {
                opt.AddPolicy("Production",
                builder => builder
                    .WithMethods("GET", "POST", "PUT", "DELETE")
                    .WithOrigins("http://otsu.dev.br")
                    .SetIsOriginAllowedToAllowWildcardSubdomains() // Permitindo subdominios
                    .WithHeaders(HeaderNames.ContentType, "x-custom-header") // Definindo headers aceitaveis
                    .AllowAnyHeader());
            });

            return services;
        }

        public static IApplicationBuilder UserMvcConfig(this IApplicationBuilder app)
        {
            app.UseCors("Development");
            app.UseHttpsRedirection();
            return app;
        }
    }
}