using Microsoft.Extensions.Logging;

namespace DeviceDataProcessor.Services
{
    /// <summary>
    /// سرویس عمومی برای ثبت لاگ در تمام قسمت‌های برنامه
    /// </summary>
    public class LoggingService : ILoggingService
    {
        private readonly ILogger<LoggingService> _logger;

        public LoggingService(ILogger<LoggingService> logger)
        {
            _logger = logger;
        }

        public void LogInformation(string message)
        {
            _logger.LogInformation(message);
        }

        public void LogInformation(string format, params object[] args)
        {
            _logger.LogInformation(format, args);
        }

        public void LogWarning(string message)
        {
            _logger.LogWarning(message);
        }

        public void LogDebug(string message)
        {
            _logger.LogDebug(message);
        }

        public void LogError(string message)
        {
            _logger.LogError(message);
        }

        public void LogError(Exception exception, string message)
        {
            _logger.LogError(exception, message);
        }

        public void LogCritical(string message)
        {
            _logger.LogCritical(message);
        }
    }

    /// <summary>
    /// Interface برای تسهیل تست واحد و DI
    /// </summary>
    public interface ILoggingService
    {
        void LogInformation(string message);
        void LogInformation(string format, params object[] args);

        void LogWarning(string message);
        void LogDebug(string message);

        void LogError(string message);
        void LogError(Exception exception, string message);
        void LogCritical(string message);
    }
}