using ExaminationSystem.EntityFrameworkCore;
using ExaminationSystem.Web.Utils.Auth;
using ExaminationSystem.Web.Utils.DependencyInjection;
using ExaminationSystem.Web.Utils.Exceptions;
using ExaminationSystem.Web.Utils.HealthChecks;
using ExaminationSystem.Web.Utils.Logging;
using ExaminationSystem.Web.Utils.Route;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.EntityFrameworkCore;


#region builder

var builder = WebApplication.CreateBuilder(args);

// 日志写入
builder.AddLoggerProvider();

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

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddProblemDetails();

var dbConnectionString = builder.Configuration.GetConnectionString("Mysql")
                         ?? throw new InvalidOperationException("未配置数据库连接字符串");

builder.Services.AddDbContext<ExaminationSystemDbContext>(options =>
{
    options.UseMySql(dbConnectionString, ServerVersion.AutoDetect(dbConnectionString));
});

// 统一异常处理
builder.Services.AddCustomException();
// 健康检查
builder.Services.AddCustomHealthChecks();
// 依赖注入
builder.Services.AddDependencyInjection();

builder.Services.AddAuth();
#endregion


#region app

var app = builder.Build();

// 统一异常处理
app.UseExceptionHandler();
// 健康检查
app.UseCustomHealthChecks();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseHttpsRedirection();

// app.UseRouting();

app.UseAuth();

app.MapControllers();

app.Run();

#endregion
