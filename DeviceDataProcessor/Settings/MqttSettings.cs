namespace DeviceDataProcessor.Settings
{
    // تنظیمات مربوط به سرور MQTT
    public class MqttSettings
    {
        public int Port { get; set; } // پورت سرور MQTT
        public string Topic { get; set; } // موضوع عمومی دستگاه‌ها

    }
}