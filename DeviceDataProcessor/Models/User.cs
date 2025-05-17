using System;
using System.ComponentModel.DataAnnotations;

namespace DeviceDataProcessor.Models
{
    /// <summary>
    /// مدل کاربر - شامل اطلاعات پایه کاربران سیستم
    /// </summary>
    public class User
    {
        /// <summary>
        /// شناسه یکتا برای هر کاربر (از نوع GUID برای امنیت بیشتر)
        /// </summary>
        public Guid Id { get; set; } = Guid.NewGuid();

        /// <summary>
        /// نام کاربری - باید منحصر به‌فرد باشد و نمی‌تواند خالی باشد
        /// </summary>
        [Required(ErrorMessage = "نام کاربری الزامی است")]
        [StringLength(100, ErrorMessage = "نام کاربری نباید بیشتر از 100 کاراکتر باشد")]
        public string Username { get; set; }

        /// <summary>
        /// رمز عبور هش شده - رمز عبور ساده هرگز ذخیره نمی‌شود
        /// </summary>
        [Required(ErrorMessage = "رمز عبور الزامی است")]
        public string PasswordHash { get; set; }

        /// <summary>
        /// نقش کاربر در سیستم (مدیر، اپراتور، مهمان)
        /// </summary>
        public UserRole Role { get; set; } = UserRole.Operator;

        /// <summary>
        /// تاریخ ثبت‌نام کاربر - به‌صورت خودکار مقداردهی می‌شود
        /// </summary>
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// آخرین زمان ورود کاربر به سیستم (در صورت وجود)
        /// </summary>
        public DateTime? LastLoginAt { get; set; }

        /// <summary>
        /// مهر امنیتی برای کنترل نسخه‌ی حساب کاربر (مثلاً در هنگام تغییر رمز عبور)
        /// </summary>
        public string SecurityStamp { get; set; } = Guid.NewGuid().ToString();

        /// <summary>
        /// وضعیت فعال بودن حساب کاربر
        /// </summary>
        public bool IsActive { get; set; } = true;

        /// <summary>
        /// وضعیت حذف منطقی حساب (برای حذف بدون از بین بردن اطلاعات)
        /// </summary>
        public bool IsDeleted { get; set; } = false;

        /// <summary>
        /// ایمیل کاربر (در صورت نیاز به احراز هویت یا بازیابی رمز عبور)
        /// </summary>
        [EmailAddress(ErrorMessage = "ایمیل وارد شده معتبر نیست")]
        public string Email { get; set; }
    }

    /// <summary>
    /// نقش‌های تعریف‌شده برای کاربران سیستم
    /// </summary>
    public enum UserRole
    {
        Admin,      // مدیر سیستم با دسترسی کامل
        Operator,   // اپراتور با دسترسی محدود
        Guest       // کاربر مهمان با کمترین سطح دسترسی
    }
}
