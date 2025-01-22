using System;

namespace DeviceDataProcessor.DTOs
{
    public class DeviceDataDto
    {
        public string DeviceId { get; set; } // شناسه دستگاه
        public DateTime Timestamp { get; set; } // زمان ثبت داده
        public double Value { get; set; } // مقدار داده
    }
}