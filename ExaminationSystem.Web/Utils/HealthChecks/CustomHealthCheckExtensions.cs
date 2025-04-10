using ExaminationSystem.EntityFrameworkCore;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Newtonsoft.Json;


namespace ExaminationSystem.Web.Utils.HealthChecks;

public static class CustomHealthCheckExtensions
{
    public static IServiceCollection AddCustomHealthChecks(this IServiceCollection services)
    {
        services
            .AddHealthChecks()
            .AddCheck<TestCustomHealthCheck>("test_custom_health_check")
            .AddDbContextCheck<ExaminationSystemDbContext>("test_db_context_health_check");
        return services;
    }
    
    
    public static IEndpointRouteBuilder UseCustomHealthChecks(this IEndpointRouteBuilder app)
    {
        app.MapHealthChecks("/health", new HealthCheckOptions
            {
                ResponseWriter = async (context, report) =>
                {
                    context.Response.ContentType = "application/json";
                    var response = new
                    {
                        status = report.Status.ToString(),
                        checks = report.Entries.Select(entry => new
                        {
                            name = entry.Key,
                            status = entry.Value.Status.ToString(),
                            description = entry.Value.Description
                        })
                    };
                    await context.Response.WriteAsync(JsonConvert.SerializeObject(response));
                }
            })
            .RequireHost("localhost");
        return app;
    }
}

