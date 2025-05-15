namespace DeviceDataProcessor.Settings
{
    // تنظیمات مربوط به RabbitMQ
    public class QueueSettings
    {
        public string HostName { get; set; } // نام میزبان برای RabbitMQ
        public string Username { get; set; } // نام کاربری (در صورت لزوم)
        public string Password { get; set; } // رمز عبور (در صورت لزوم)
        public string QueueName { get; set; } // نام صف
    }
}