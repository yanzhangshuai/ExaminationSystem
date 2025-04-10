namespace ExaminationSystem.Web.Utils.Logger;

public static class LoggerExtensions
{
    public static WebApplicationBuilder AddLoggerProvider(this WebApplicationBuilder builder)
    {
        builder.Logging.AddProvider(new FileLoggerProvider("./logs/app.log"));
        return builder;
    }
}