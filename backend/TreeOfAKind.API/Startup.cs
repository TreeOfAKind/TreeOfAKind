using System;
using System.Configuration;
using System.Linq;
using AspNetCore.Firebase.Authentication.Extensions;
using Hellang.Middleware.ProblemDetails;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.UserSecrets;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TreeOfAKind.API.Configuration;
using TreeOfAKind.Application.Configuration.Validation;
using TreeOfAKind.API.SeedWork;
using TreeOfAKind.Application.Configuration;
using TreeOfAKind.Application.Configuration.Emails;
using TreeOfAKind.Domain.SeedWork;
using TreeOfAKind.Infrastructure;
using TreeOfAKind.Infrastructure.Caching;
using Serilog;
using Serilog.Formatting.Compact;
using TreeOfAKind.Application.Configuration.Authorization;
using TreeOfAKind.Infrastructure.Database;

[assembly: UserSecretsId("54e8eb06-aaa1-4fff-9f05-3ced1cb623c2")]
namespace TreeOfAKind.API
{
    public class Startup
    {
        private const string TreesConnectionString = "TreesConnectionString";

        private static ILogger _logger;

        private readonly IConfiguration _configuration;

        public Startup(IWebHostEnvironment env)
        {
            _logger = ConfigureLogger();
            _logger.Information("Logger configured");

            this._configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json")
                .AddJsonFile($"hosting.{env.EnvironmentName}.json")
                .AddUserSecrets<Startup>()
                .AddEnvironmentVariables()
                .Build();
        }

        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddApplicationInsightsTelemetry();

            services.AddControllers();

            services.AddMemoryCache();

            services.AddFirebaseAuthentication(_configuration["FirebaseAuthentication:Issuer"], _configuration["FirebaseAuthentication:Audience"]);

            services.AddSwaggerDocumentation();

            services.AddProblemDetails(x =>
            {
                x.Map<InvalidCommandException>(ex => new InvalidCommandProblemDetails(ex));
                x.Map<BusinessRuleValidationException>(ex => new BusinessRuleValidationExceptionProblemDetails(ex));
                x.Map<UnauthorizedException>(ex => new UnauthorizedProblemDetails(ex));
            });


            services.AddHttpContextAccessor();
            return InitializeAutofac(services);
        }

        private IServiceProvider InitializeAutofac(IServiceCollection services)
        {
            var serviceProvider = services.BuildServiceProvider();

            IExecutionContextAccessor executionContextAccessor =
                new ExecutionContextAccessor(serviceProvider.GetService<IHttpContextAccessor>());

            var children = this._configuration.GetSection("Caching").GetChildren();
            var cachingConfiguration = children.ToDictionary(child => child.Key, child => TimeSpan.Parse(child.Value));
            var emailsSettings = _configuration.GetSection("EmailsSettings").Get<EmailsSettings>();
            var memoryCache = serviceProvider.GetService<IMemoryCache>();
            return ApplicationStartup.Initialize(
                services,
                this._configuration[TreesConnectionString],
                new MemoryCacheStore(memoryCache, cachingConfiguration),
                null,
                emailsSettings,
                _logger,
                executionContextAccessor);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, TreesContext context)
        {
            app.UseCors(cfg =>
            {
                (env.IsProduction() ? cfg.WithOrigins().AllowCredentials() : cfg.AllowAnyOrigin()) // TODO: Add web domain
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .SetPreflightMaxAge(TimeSpan.FromMinutes(60));
            });

            app.UseMiddleware<CorrelationMiddleware>();

            app.UseAuthentication();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseProblemDetails();
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });

            app.UseSwaggerDocumentation();
            
            context.Database.Migrate();
        }

        private static ILogger ConfigureLogger()
        {
            return new LoggerConfiguration()
                .Enrich.FromLogContext()
                .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] [{Context}] {Message:lj}{NewLine}{Exception}")
                .WriteTo.RollingFile(new CompactJsonFormatter(), "logs/logs")
                .CreateLogger();
        }
    }
}
