using ExaminationSystem.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MySqlConnector;

// 使用.NET通用主机构建器
var host = Host.CreateDefaultBuilder(args)
    
    .ConfigureLogging(logging =>
    {
        logging.ClearProviders();
        logging.AddConsole();
        logging.AddFilter(level => level >= LogLevel.Information);
    })
    .ConfigureServices((context, services) =>
    {

        var dbConnectionString = context.Configuration.GetConnectionString("Mysql");
        services.AddDbContext<ExaminationSystemDbContext>(options =>
        {
            options.UseMySql(dbConnectionString, ServerVersion.AutoDetect(dbConnectionString),
                x => x.MigrationsAssembly("ExaminationSystem.Migrator"));
        });
    })
    .Build();

// 迁移执行与错误处理
try
{
    using var scope = host.Services.CreateScope();
    var services = scope.ServiceProvider;
    var logger = services.GetRequiredService<ILogger<Program>>();
    var context = services.GetRequiredService<ExaminationSystemDbContext>();

    logger.LogInformation("开始数据库迁移...");

    // 验证数据库连接,如果没有数据库则创建一个空的数据库
    if (!await context.Database.CanConnectAsync())
    {
        logger.LogWarning("目标数据库不存在，开始初始化...");
        if (context.Database.IsMySql())
        {
            var databaseName = context.Database.GetDbConnection().Database;
            logger.LogInformation("正在创建MySQL数据库 '{Database}'...", databaseName);

            await using var masterConn = new MySqlConnection(
                context.Database.GetConnectionString()?.Replace(databaseName, "mysql"));
            await masterConn.OpenAsync();

            await using var createCmd = new MySqlCommand(
                $"CREATE DATABASE IF NOT EXISTS `{databaseName}` CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci;", 
                masterConn);
            await createCmd.ExecuteNonQueryAsync();
        }
        else
        {
            throw new Exception("不支持的数据库类型");
        }
    }
    else
    {
        logger.LogInformation("数据库连接成功");
    }

    // 获取待应用的迁移
    var pendingMigrations = await context.Database.GetPendingMigrationsAsync();
    var migrations = pendingMigrations as string[] ?? pendingMigrations.ToArray();
    if (!migrations.Any())
    {
        logger.LogInformation("没有待应用的迁移");
        return 0;
    }

    logger.LogInformation("待应用迁移: \n- {Migrations}",
        string.Join("\n- ", migrations));

    // 执行迁移
    await context.Database.MigrateAsync();

    logger.LogInformation("数据库迁移成功完成");
    return 0;
}
catch (Exception ex)
{
    var logger = host.Services.GetRequiredService<ILogger<Program>>();
    logger.LogError(ex, "数据库迁移过程中发生错误");
    return 1;
}
finally
{
    await host.StopAsync();
}

// logger.LogInformation("数据库连接失败,正在创建数据库...");
// await context.Database.EnsureCreatedAsync();
//         
// context.
//     logger.LogInformation("数据库创建成功");