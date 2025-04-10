namespace ExaminationSystem.Web.Utils.Logging;

public class FileLoggerOptions
{
    public string LogPath { get; set; }
    public bool Append { get; set; } = true;
    public long FileSizeLimitBytes { get; set; } = 10 * 1024 * 1024; // 10MB
    public LogLevel MinLevel { get; set; } = LogLevel.Information; // 默认最低级别
    public Dictionary<string, LogLevel> CategoryFilters { get; set; } // 各类别级别
}
