using System;
using System.ComponentModel.DataAnnotations;

namespace DeviceDataProcessor.Models
{
    /// <summary>
    /// مدل لاگ پاسخ API برای ذخیره نتایج پاسخ‌ها در دیتابیس
    /// </summary>
    public class ApiResponseLog
    {
        /// <summary>
        /// شناسه یکتا برای هر لاگ
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// وضعیت موفقیت عملیات (true: موفق، false: ناموفق)
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// پیام مرتبط با نتیجه عملیات (مانند خطا یا توضیحات)
        /// </summary>
        [StringLength(1000, ErrorMessage = "پیام نمی‌تواند بیش از 1000 کاراکتر باشد")]
        public string Message { get; set; }

        /// <summary>
        /// داده‌های برگشتی به صورت رشته JSON (در صورت وجود)
        /// </summary>
        public string DataJson { get; set; }

        /// <summary>
        /// زمان ایجاد این رکورد لاگ به صورت UTC
        /// </summary>
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
