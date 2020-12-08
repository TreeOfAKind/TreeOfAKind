using System;
using System.IO;
using System.Reflection;
using System.Text.Json;
using MicroElements.Swashbuckle.NodaTime;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace TreeOfAKind.API.Configuration
{
    internal static class SwaggerExtensions
    {
        internal static IServiceCollection AddSwaggerDocumentation(this IServiceCollection services)
        {
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
                {
                    Title = "Tree Of A Kind API",
                    Version = "v1",
                    Description = "",
                });
                var jsonSerializerOptions = new JsonSerializerOptions();
                Startup.ConfigureSerializerSettings(jsonSerializerOptions);
                options.ConfigureForNodaTimeWithSystemTextJson(jsonSerializerOptions);
                var baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
                var commentsFileName = Assembly.GetExecutingAssembly().GetName().Name + ".XML";
                var commentsFile = Path.Combine(baseDirectory, commentsFileName);
                options.IncludeXmlComments(commentsFile);
            });

            return services;
        }

        internal static IApplicationBuilder UseSwaggerDocumentation(this IApplicationBuilder app)
        {
            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Tree Of A Kind API");
            });

            return app;
        }
    }
}
