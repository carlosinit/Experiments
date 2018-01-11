using NLog;

namespace OwinRequestInfo
{
    public interface ILogger
    {
        void Log(LogLevel level, string message);
    }
}