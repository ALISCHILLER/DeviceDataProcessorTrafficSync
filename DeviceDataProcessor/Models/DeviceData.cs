using System;

namespace DeviceDataProcessor.Models
{
    /// <summary>
    /// مدل داده‌های دستگاه – شامل تمامی اطلاعات ثبت شده از یک دستگاه ترافیکی
    /// </summary>
    public class DeviceData
    {
        public int Id { get; set; } // شناسه منحصر به فرد داده
        public string DeviceId { get; set; } // شناسه دستگاهی که داده را ارسال کرده است
        public DateTime Timestamp { get; set; } // زمان ثبت داده (UTC)
        public double Value { get; set; } // مقدار اصلی داده (اختیاری – مثلاً سرعت)
        public DataQuality Quality { get; set; } // کیفیت داده: Good, Medium, Bad
        public virtual Device Device { get; set; } // ارتباط با دستگاه

        // --- فیلدهای تخصصی ترافیک ---
        public TimeSpan ST { get; set; } // Start Time – hh:mm:ss
        public TimeSpan ET { get; set; } // End Time – hh:mm:ss
        public int C1 { get; set; } // Class 1 Flow
        public int C2 { get; set; } // Class 2 Flow
        public int C3 { get; set; } // Class 3 Flow
        public int C4 { get; set; } // Class 4 Flow
        public int C5 { get; set; } // Class 5 Flow
        public double ASP { get; set; } // Average Speed
        public int SO { get; set; } // Speeding Offence
        public int OO { get; set; } // Overtaking Offence
        public int ESD { get; set; } // Exceeding Safe Distance
    }

    /// <summary>
    /// کیفیت داده – مشخص کننده وضعیت فنی دستگاه
    /// </summary>
    public enum DataQuality
    {
        Good,
        Medium,
        Bad
    }
}