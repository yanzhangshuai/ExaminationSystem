using System.Text;

namespace ExaminationSystem.Web.Utils.Logging;

public sealed class FileLogger(string filePath, bool append, long fileSizeLimitBytes, LogLevel minLogLevel)
    : ILogger
{
    private readonly bool _append = append;
    private static readonly object Lock = new();

    public IDisposable BeginScope<TState>(TState state) where TState : notnull
    {
        return new FileLoggerScope(state);
    }

    public bool IsEnabled(LogLevel logLevel)
    {
        return logLevel >= minLogLevel;
    }

    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
    {
        if (!IsEnabled(logLevel))
        {
            return;
        }

        var sb = new StringBuilder();
        sb.AppendLine($"{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff} [{logLevel}] ({eventId.Name}/{eventId.Id})");

        if (state is IReadOnlyCollection<KeyValuePair<string, object>> scopes)
        {
            foreach (var scope in scopes)
            {
                sb.AppendLine($"=> {scope.Key}: {scope.Value}");
            }
        }

        // 日志内容
        sb.AppendLine($"Message: {formatter(state, exception)}");

        if (exception != null)
        {
            sb.AppendLine($"Exception: {exception}");
        }

        lock (Lock)
        {
            // 检查并限制文件大小
            var fileInfo = new FileInfo(filePath);
            if (fileInfo.Exists && fileInfo.Length > fileSizeLimitBytes)
            {
                File.Move(filePath, $"{filePath}.{DateTime.Now:yyyyMMddHHmmss}.backup");
            }

            // 确保目录存在
            var directory = Path.GetDirectoryName(filePath);
            if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            File.AppendAllText(filePath, sb.ToString(), Encoding.UTF8);
        }
    }
}

