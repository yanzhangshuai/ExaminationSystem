using Microsoft.Extensions.Options;

namespace ExaminationSystem.Web.Utils.Logging;

public static class LoggerExtensions
{
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
}