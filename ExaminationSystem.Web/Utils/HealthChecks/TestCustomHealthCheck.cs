using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace ExaminationSystem.Web.Utils.HealthChecks;

public class TestCustomHealthCheck: IHealthCheck
{
    public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = new CancellationToken())
    {
        // 这是一个测试 健康检查
        var isHealthy = false;

        // ...

        if (isHealthy)
        {
            return Task.FromResult(
                HealthCheckResult.Healthy("这是一个测试成功的健康检查！！！"));
        }

        return Task.FromResult(
            new HealthCheckResult(
                context.Registration.FailureStatus, "这是一个测试失败的健康检查！！！"));
    }
}