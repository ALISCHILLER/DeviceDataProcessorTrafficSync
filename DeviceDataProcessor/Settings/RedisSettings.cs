namespace DeviceDataProcessor.Settings
{
    // تنظیمات مربوط به Redis
    public class RedisSettings
    {
        public string ConnectionString { get; set; } // رشته اتصال Redis
        public TimeSpan ExpiryTime => TimeSpan.FromMinutes(10); // زمان انقضا (پیش‌فرض 10 دقیقه)
    }
}