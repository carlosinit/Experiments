using NLog;
using System;

namespace OwinRequestInfo
{
    internal class NLogLogger : ILogger
    {
        private Type callerType;
        private Logger logger;
        private string correlationId;

        public NLogLogger(Type callerType)
        {
            this.callerType = callerType;
            logger = LogManager.GetLogger(callerType.FullName);
        }

        public NLogLogger(Type callerType, string correlationId)
            : this(callerType)
        {
            this.correlationId = correlationId;
        }

        public void Log(LogLevel level, string message)
        {
            var eventLog = new LogEventInfo(level, callerType.FullName, message);
            eventLog.Properties.Add("correlation-id", correlationId);
            logger.Log(eventLog);
        }
    }
}