using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace FunctionApp1.Tests.Helpers
{
    public enum LoggerTypes
    {
        Null,
        List
    }

    public static class LoggerHelper
    {
        public static ILogger CreateLogger(
            LoggerTypes type = LoggerTypes.Null)
        {
            ILogger logger;

            if (type == LoggerTypes.List)
            {
                logger = new ListLogger();
            }
            else
            {
                logger = NullLoggerFactory.Instance.CreateLogger("Null Logger");
            }

            return logger;
        }
    }
}
