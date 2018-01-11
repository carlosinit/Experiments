using Microsoft.Owin;

namespace OwinRequestInfo
{
    public class LoggerContext
    {
        public string CorrelationId { get; private set; }

        public LoggerContext()
        {
            
            //CorrelationId = correlationId;
        }
    }
}