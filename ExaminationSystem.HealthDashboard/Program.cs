var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddHealthChecksUI(options =>
{
    var endpoints = builder.Configuration.GetSection("HealthCheckEndpoints");
    foreach (var endpoint in endpoints.GetChildren())
    {
        if (endpoint.Value != null)
            options.AddHealthCheckEndpoint(endpoint.Key, endpoint.Value)
                .SetEvaluationTimeInSeconds(30) // 检查间隔
                .SetMinimumSecondsBetweenFailureNotifications(60); // 失败通知间隔
    }



})
.AddInMemoryStorage();

var app = builder.Build();


app.UseHttpsRedirection();

app.MapHealthChecksUI(options =>
{
    options.UIPath = "/dashboard";
}).RequireHost("localhost");
app.Run();

