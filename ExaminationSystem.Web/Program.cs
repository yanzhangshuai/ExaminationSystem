using ExaminationSystem.EntityFrameworkCore;
using ExaminationSystem.Web.Utils.DependencyInjection;
using ExaminationSystem.Web.Utils.Exceptions;
using ExaminationSystem.Web.Utils.HealthChecks;
using Microsoft.EntityFrameworkCore;


#region builder

var builder = WebApplication.CreateBuilder(args);

// builder.AddLoggerProvider();
// Add services to the container.


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
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

#endregion


#region app

var app = builder.Build();

// if (app.Environment.IsDevelopment())
// {
//     app.UseSwagger();
//     app.UseSwaggerUI();
// }

//TODO: 测试，生产环境启用swagger
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

// 统一异常处理
app.UseExceptionHandler();
// 健康检查
app.UseCustomHealthChecks();

app.Run();

#endregion