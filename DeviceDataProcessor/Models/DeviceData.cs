using System;

namespace DeviceDataProcessor.Models
{
    /// <summary>
    /// مدل داده‌های دستگاه - شامل تمامی اطلاعات ثبت شده از یک دستگاه ترافیکی
    /// </summary>
    public class DeviceData
    {
        /// <summary>
        /// شناسه منحصر به فرد داده
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// شناسه دستگاهی که داده را ارسال کرده است
        /// </summary>
        public string DeviceId { get; set; }

        /// <summary>
        /// زمان ثبت داده (UTC)
        /// </summary>
        public DateTime Timestamp { get; set; }

        /// <summary>
        /// مقدار اصلی داده (اختیاری – مثلاً سرعت خودرو)
        /// </summary>
        public double Value { get; set; }

        /// <summary>
        /// کیفیت داده – مشخص کننده وضعیت فنی دستگاه (Good, Medium, Bad)
        /// </summary>
        public DataQuality Quality { get; set; }

        /// <summary>
        /// ارتباط با دستگاه مربوطه
        /// </summary>
        public virtual Device Device { get; set; }

        // --- فیلدهای تخصصی ترافیک ---

        /// <summary>
        /// زمان شروع (Start Time) – فرمت hh:mm:ss
        /// </summary>
        public TimeSpan ST { get; set; }

        /// <summary>
        /// زمان پایان (End Time) – فرمت hh:mm:ss
        /// </summary>
        public TimeSpan ET { get; set; }

        /// <summary>
        /// تعداد خودروهای کلاس 1 (سواری، وانت)
        /// </summary>
        public int C1 { get; set; }

        /// <summary>
        /// تعداد خودروهای کلاس 2 (کامیونت، مینی‌بوس)
        /// </summary>
        public int C2 { get; set; }

        /// <summary>
        /// تعداد خودروهای کلاس 3 (کامیون دو محور)
        /// </summary>
        public int C3 { get; set; }

        /// <summary>
        /// تعداد خودروهای کلاس 4 (اتوبوس)
        /// </summary>
        public int C4 { get; set; }

        /// <summary>
        /// تعداد خودروهای کلاس 5 (کامیون سه محور)
        /// </summary>
        public int C5 { get; set; }

        /// <summary>
        /// سرعت متوسط خودروها (Average Speed)
        /// </summary>
        public double ASP { get; set; }

        /// <summary>
        /// تعداد تخلف‌های سرعت غیرمجاز (Speeding Offence)
        /// </summary>
        public int SO { get; set; }

        /// <summary>
        /// تعداد تخلف‌های سبقت غیرمجاز (Overtaking Offence)
        /// </summary>
        public int OO { get; set; }

        /// <summary>
        /// تعداد تخلف‌های فاصله غیرمجاز (Exceeding Safe Distance)
        /// </summary>
        public int ESD { get; set; }
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