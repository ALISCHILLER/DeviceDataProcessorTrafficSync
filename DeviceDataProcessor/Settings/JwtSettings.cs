namespace DeviceDataProcessor.Settings
{
    // تنظیمات JWT برای احراز هویت
    public class JwtSettings
    {
        public string Secret { get; set; } // کلید مخفی JWT
        public int ExpiryInHours { get; set; } // زمان انقضای توکن به ساعت
    }
}