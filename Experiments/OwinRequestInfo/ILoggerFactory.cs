using System;

namespace OwinRequestInfo
{
    internal interface ILoggerFactory
    {
        ILogger CreateLogger(Type caller);
    }
}