using DeviceDataProcessor.Models;
using System;

namespace DeviceDataProcessor.DTOs
{
    /// <summary>
    /// DTO داده‌های دستگاه – شامل تمام فیلدهای لازم برای API و اعتبارسنجی
    /// </summary>
    public class DeviceDataDto
    {
        /// <summary>
        /// شناسه دستگاه ارسال‌کننده داده
        /// </summary>
        public string DeviceId { get; set; }

        /// <summary>
        /// زمان ثبت داده (شامل تاریخ و ساعت)
        /// </summary>
        public DateTime Timestamp { get; set; }

        /// <summary>
        /// زمان شروع بازه (فقط زمان روز - hh:mm:ss)
        /// </summary>
        public TimeSpan ST { get; set; }

        /// <summary>
        /// زمان پایان بازه (فقط زمان روز - hh:mm:ss)
        /// </summary>
        public TimeSpan ET { get; set; }

        /// <summary>
        /// تعداد جریان کلاس 1
        /// </summary>
        public int C1 { get; set; }

        /// <summary>
        /// تعداد جریان کلاس 2
        /// </summary>
        public int C2 { get; set; }

        /// <summary>
        /// تعداد جریان کلاس 3
        /// </summary>
        public int C3 { get; set; }

        /// <summary>
        /// تعداد جریان کلاس 4
        /// </summary>
        public int C4 { get; set; }

        /// <summary>
        /// تعداد جریان کلاس 5
        /// </summary>
        public int C5 { get; set; }

        /// <summary>
        /// سرعت متوسط (اختیاری)
        /// </summary>
        public double? ASP { get; set; }

        /// <summary>
        /// تعداد تخلفات سرعت (اختیاری)
        /// </summary>
        public int? SO { get; set; }

        /// <summary>
        /// تعداد تخلفات سبقت (اختیاری)
        /// </summary>
        public int? OO { get; set; }

        /// <summary>
        /// تعداد تخلفات فاصله غیرمجاز (اختیاری)
        /// </summary>
        public int? ESD { get; set; }

        /// <summary>
        /// مقدار اصلی داده (مثلاً سرعت) (اختیاری)
        /// </summary>
        public double? Value { get; set; }

        /// <summary>
        /// کیفیت داده – مقدار باید یکی از مقادیر enum DataQuality باشد (اختیاری)
        /// </summary>
        public DataQuality? Quality { get; set; }
    }
}
