namespace DeviceDataProcessor.Models
{
    // مدل داده‌های دستگاه که اطلاعات مربوط به داده‌های ارسال شده از دستگاه‌ها را ذخیره می‌کند
    public class DeviceData
    {
        public int Id { get; set; } // شناسه داده دستگاه
        public string DeviceId { get; set; } // شناسه دستگاه
        public DateTime Timestamp { get; set; } // زمان ثبت داده
        public double Value { get; set; } // مقدار داده
        public string Quality { get; set; } // کیفیت داده (مثلاً خوب، متوسط، بد)
    }
}