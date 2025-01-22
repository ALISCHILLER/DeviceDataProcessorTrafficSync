namespace DeviceDataProcessor.Settings
{
    public class JwtSettings
    {
        public string Secret { get; set; } // کلید مخفی JWT
        public int ExpiryInHours { get; set; } // زمان انقضای توکن به ساعت
    }
}