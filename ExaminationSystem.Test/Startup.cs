using ExaminationSystem.EntityFrameworkCore;
using ExaminationSystem.Test.Utils.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Xunit.DependencyInjection.Logging;

namespace ExaminationSystem.Test;

public class Startup
{
    public void ConfigureHost(IHostBuilder hostBuilder)
    {
    }
    
    public void ConfigureServices(IServiceCollection services, HostBuilderContext context)
    {
        
        services.AddDbContext<ExaminationSystemDbContext>(options =>
        {
            // var dbConnectionString = context.Configuration.GetConnectionString("Mysql");
            var dbConnectionString = "Server=localhost;Database=exam_sym;User=root;Password=123456;";
            options.UseMySql(dbConnectionString, ServerVersion.AutoDetect(dbConnectionString));
        });


        // 依赖注入
        services.AddDependencyInjection();
        
        services.AddLogging(lb => lb.AddXunitOutput());
        context.HostingEnvironment.EnvironmentName = "test";
        context.Configuration.GetChildren();
    }

    public void Configure()
    {

    }

}