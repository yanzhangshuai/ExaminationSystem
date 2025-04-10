### 项目结构
* ExaminationSystem.EntityFrameCore     EF仓储层
* ExaminationSystem.Model               实体模型层
* ExaminationSystem.Application         应用层
* ExaminationSystem.Web                 WebAPI项目
* ExaminationSystem.Test                单元测试项目


`docker-compose.yml` 文件提供了一键部署 `mysql`服务、web服务。 `mysql`服务暴露了 `4315` 端口。同时`Mysql` 服务启动时，会创建`Student`表。

单元测试，Web调试默认使用这个数据库
```json 
{
 "ConnectionStrings": {
    "Mysql": "server=localhost;port=4315;database=exam_sym;userid=root;password=123456;"
  }
}
```

### 介绍
项目中则使用了 `EFCore` 操作数据库。仓储使用了`EntityFrameworkCore.Data.UnitOfWork`

`Web 服务` 提供了两个路由，
1. /student/list 获取学生
2. /student/generate 随机生成20-50学生，会将数据库之前的数据清空 


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
健康检查有两个，一个是模拟测试，一个是数据库
```c#
public static IServiceCollection AddCustomHealthChecks(this IServiceCollection services)
    {
        services
            .AddHealthChecks()
            .AddCheck<TestCustomHealthCheck>("test_custom_health_check")
            .AddDbContextCheck<ExaminationSystemDbContext>("test_db_context_health_check");
        return services;
    }
```