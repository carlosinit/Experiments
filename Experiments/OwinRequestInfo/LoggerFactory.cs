using System;

namespace OwinRequestInfo
{
    internal class LoggerFactory : ILoggerFactory
    {
        public LoggerFactory(LoggerContext context)
        {

        }

        public ILogger CreateLogger(Type caller)
        {


            return new NLogLogger(caller);
        }
    }
}