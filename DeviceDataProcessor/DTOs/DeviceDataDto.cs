using System;

namespace DeviceDataProcessor.DTOs
{
    /// <summary>
    /// DTO داده‌های دستگاه – شامل تمام فیلدهای لازم برای API و اعتبارسنجی
    /// </summary>
    public class DeviceDataDto
    {
        /// <summary>
        /// شناسه دستگاه – مثل "D123" (الزامی)
        /// </summary>
        public string DeviceId { get; set; }

        /// <summary>
        /// زمان ثبت داده (UTC) – الزامی
        /// </summary>
        public DateTime Timestamp { get; set; }

        /// <summary>
        /// زمان شروع – فرمت hh:mm:ss (الزامی)
        /// </summary>
        public TimeSpan ST { get; set; }

        /// <summary>
        /// زمان پایان – فرمت hh:mm:ss (الزامی)
        /// </summary>
        public TimeSpan ET { get; set; }

        /// <summary>
        /// تعداد خودروهای کلاس 1 (سواری، وانت) – الزامی (باید ≥ 0 باشد)
        /// </summary>
        public int C1 { get; set; }

        /// <summary>
        /// تعداد خودروهای کلاس 2 (کامیونت، مینی‌بوس) – الزامی (باید ≥ 0 باشد)
        /// </summary>
        public int C2 { get; set; }

        /// <summary>
        /// تعداد خودروهای کلاس 3 (کامیون دو محور) – الزامی (باید ≥ 0 باشد)
        /// </summary>
        public int C3 { get; set; }

        /// <summary>
        /// تعداد خودروهای کلاس 4 (اتوبوس) – الزامی (باید ≥ 0 باشد)
        /// </summary>
        public int C4 { get; set; }

        /// <summary>
        /// تعداد خودروهای کلاس 5 (کامیون سه محور) – الزامی (باید ≥ 0 باشد)
        /// </summary>
        public int C5 { get; set; }

        /// <summary>
        /// سرعت متوسط خودروها – اختیاری (باید > 0 باشد اگر موجود باشد)
        /// </summary>
        public double? ASP { get; set; }

        /// <summary>
        /// تعداد تخلف‌های سرعت غیرمجاز – اختیاری (باید ≥ 0 باشد)
        /// </summary>
        public int? SO { get; set; }

        /// <summary>
        /// تعداد تخلف‌های سبقت غیرمجاز – اختیاری (باید ≥ 0 باشد)
        /// </summary>
        public int? OO { get; set; }

        /// <summary>
        /// تعداد تخلف‌های فاصله غیرمجاز – اختیاری (باید ≥ 0 باشد)
        /// </summary>
        public int? ESD { get; set; }
    }
}