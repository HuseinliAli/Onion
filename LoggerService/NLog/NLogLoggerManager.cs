using Contracts.Logging;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoggerService.NLog
{
    public class NLogLoggerManager : ILoggerManager
    {
        private static ILogger _logger = LogManager.GetCurrentClassLogger(); 
        public NLogLoggerManager()
        {
            
        }
        public void LogDebug(string message) 
            => _logger.Debug(message);

        public void LogError(string message)
           => _logger.Error(message);

        public void LogInfo(string message)
            => _logger.Info(message);

        public void LogWarning(string message)
            => _logger.Warn(message);
    }
}
