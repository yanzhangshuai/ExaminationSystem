namespace ExaminationSystem.Web.Utils.Logging;

public sealed class FileLoggerScope(object state) : IDisposable
{
    private readonly object _state = state;

    public void Dispose() { }
}