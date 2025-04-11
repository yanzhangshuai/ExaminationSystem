### 项目结构
* ExaminationSystem.EntityFrameCore     EF仓储层
* ExaminationSystem.Model               实体模型层
* ExaminationSystem.Application         应用层
* ExaminationSystem.Web                 WebAPI项目
* ExaminationSystem.Test                单元测试项目
* ExaminationSystem.HealthDashboard     健康检查看板项目


`docker-compose.yml` 文件提供了一键部署 `mysql`服务、web服务。 `mysql`服务暴露了 `4315` 端口。同时`Mysql` 服务启动时，会创建`Student`表。

单元测试，调试Web项目 都默认使用这个数据库
```json 
{
 "ConnectionStrings": {
    "Mysql": "server=127.0.0.1;port=4315;database=exam_sym;userid=test;password=test;"
  }
}
```

### 项目路由
部署完毕后路由地址 **wsl2无法localhost**
* http://127.0.0.1:4317/swagger/index.html             web项目swagger
* http://127.0.0.1:4318/dashboard#/healthchecks        健康检查数据看板

### 介绍
项目中则使用了 `EFCore` 操作数据库。仓储使用 `EntityFrameworkCore.Data.UnitOfWork`

Web项目 提供了两个路由，
1. /student/list        获取考生
2. /student/generate    随机生成20-50考生，会清空之前的考生 


#### 路由转小写
路由增加了`GetAll`转 `get-all` 功能
```c#
builder.Services.AddControllers()
    .AddMvcOptions(o => 
    {
        // 路由转小写
        o.Conventions.Add(new RouteTokenTransformerConvention(new KebabCaseParameterTransformer()));
    });
builder.Services.Configure<RouteOptions>(options =>
{
    options.LowercaseUrls = true; // 强制全URL小写
    options.LowercaseQueryStrings = true; // 查询参数小写
    options.AppendTrailingSlash = false; // 移除末尾斜杠（可选）
});
builder.Services.AddSwaggerGen(c =>
{
    c.DocumentFilter<KebabCaseSwaggerFilter>();
});
```

#### 日志写入本地
自定义了一个`FileLoggerProvider` 提供了日志写入本地功能
```c#
public static WebApplicationBuilder AddLoggerProvider(this WebApplicationBuilder builder)
    {
        var fileOptions = builder.Configuration.GetSection("FileLogger").Get<FileLoggerOptions>();

        var filterOptions = builder.Logging.Services.BuildServiceProvider()
            .GetRequiredService<IOptionsMonitor<LoggerFilterOptions>>()
            .CurrentValue;

        if (fileOptions == null) return builder;
        
        var fileProvider = new FileLoggerProvider(
            new OptionsWrapper<FileLoggerOptions>(fileOptions),
            new OptionsWrapper<LoggerFilterOptions>(filterOptions)
        );
        builder.Logging.AddProvider(fileProvider);
        
        return builder;
    }
```


#### 全局异常处理
增加了两个全局异常，一个是自定义异常，一个是系统异常
```c#
public static IServiceCollection AddCustomException(this IServiceCollection services)
    {
        // 注册自定义异常处理
        services.AddExceptionHandler<CustomExceptionHandler>();
        services.AddExceptionHandler<SystemExceptionHandle>();

        return services;
    }
```

#### 健康检查
健康检查有四个：1.模拟测试，2.redis，3.http，4.数据库
```c#
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

```
