namespace DeviceDataProcessor.Models
{
    public class DeviceData
    {
        public int Id { get; set; } // شناسه داده دستگاه
        public string DeviceId { get; set; } // شناسه دستگاه
        public DateTime Timestamp { get; set; } // زمان ثبت داده
        public double Value { get; set; } // مقدار داده
    }
}