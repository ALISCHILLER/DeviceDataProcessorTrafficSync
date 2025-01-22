using Microsoft.Extensions.Logging;

namespace DeviceDataProcessor.Services
{
    public class LoggingService
    {
        private readonly ILogger<LoggingService> _logger; // لاگر

        public LoggingService(ILogger<LoggingService> logger)
        {
            _logger = logger; // دریافت لاگر
        }

        public void LogInformation(string message)
        {
            _logger.LogInformation(message); // ثبت اطلاعات
        }

        public void LogError(string message)
        {
            _logger.LogError(message); // ثبت خطا
        }
    }
}