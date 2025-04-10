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
        hostBuilder.ConfigureAppConfiguration((context, config) => 
        {
            config.AddJsonFile("appsettings.json", optional: false)
                .AddJsonFile($"appsettings.{context.HostingEnvironment.EnvironmentName}.json", optional: true);
        });
    }
    
    public void ConfigureServices(IServiceCollection services, HostBuilderContext context)
    {
        
        // 获取已加载的配置
        var configuration = context.Configuration;
        
        // 注册配置到DI容器（可选）
        services.AddSingleton(configuration);
        
        var dbConnectionString = context.Configuration.GetConnectionString("Mysql")
                                 ?? throw new InvalidOperationException("未配置数据库连接字符串");
        
        services.AddDbContext<ExaminationSystemDbContext>(options =>
        {
            var dbConnectionString = "server=localhost;port=4315;database=exam_sym;userid=root;password=123456;";
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