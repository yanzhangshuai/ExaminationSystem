using ExaminationSystem.EntityFrameworkCore;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Diagnostics.HealthChecks;


namespace ExaminationSystem.Web.Utils.HealthChecks;

public static class CustomHealthCheckExtensions
{
    public static IServiceCollection AddCustomHealthChecks(this IServiceCollection services)
    {
        services
            .AddHealthChecks()
            .AddCheck<TestCustomHealthCheck>(
                name: "test_health_check",
                HealthStatus.Unhealthy,
                tags: ["test"]
                )
            .AddRedis(
                "127.0.0.1:6379", 
                name: "redis_health_check",
                HealthStatus.Unhealthy,
                tags: ["redis"]
                )
            .AddUrlGroup(
                new Uri("https://baidu.com"),
                name: "api_health_check",
                HealthStatus.Unhealthy,
                tags: ["baidu"]
            )
            .AddDbContextCheck<ExaminationSystemDbContext>(
                name: "database_health_check",
                HealthStatus.Unhealthy,
                tags: ["database"]
            );
        return services;
    }


    public static IEndpointRouteBuilder UseCustomHealthChecks(this IEndpointRouteBuilder app)
    {
        app.MapHealthChecks("/healthz", new HealthCheckOptions
        {
            Predicate = _ => true,
            ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse,
        });
        return app;
    }
}