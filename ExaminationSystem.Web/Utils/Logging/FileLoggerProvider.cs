using Microsoft.Extensions.Options;

namespace ExaminationSystem.Web.Utils.Logging;
public class FileLoggerProvider : ILoggerProvider
{
    private readonly FileLoggerOptions _options;
    private readonly Dictionary<string, LogLevel> _categoryFilters;

    public FileLoggerProvider(IOptions<FileLoggerOptions> options, IOptions<LoggerFilterOptions> filterOptions)
    {
        _options = options.Value;
        
        _categoryFilters = new Dictionary<string, LogLevel>();
        foreach (var rule in filterOptions.Value.Rules)
        {
            // 匹配 Default 规则和其他类别
            if (rule.CategoryName == null || rule.CategoryName == "Default")
            {
                _options.MinLevel = rule.LogLevel ?? _options.MinLevel;
            }
            else
            {
                _categoryFilters[rule.CategoryName] = rule.LogLevel ?? _options.MinLevel;
            }
        }
    }

    public ILogger CreateLogger(string categoryName)
    {
        LogLevel minLevel = _options.MinLevel;
        
        // 检查是否匹配特定类别（如 "Microsoft.AspNetCore"）
        foreach (var filter in _categoryFilters)
        {
            if (categoryName.StartsWith(filter.Key, StringComparison.OrdinalIgnoreCase))
            {
                minLevel = filter.Value;
                break;
            }
        }

        return new FileLogger(
            _options.LogPath,
            _options.Append,
            _options.FileSizeLimitBytes,
            minLevel
        );
    }

    public void Dispose()
    {
        // 清理资源
    }
}
