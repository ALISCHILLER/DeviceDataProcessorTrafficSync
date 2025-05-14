using System;

namespace DeviceDataProcessor.Models
{
    /// <summary>
    /// مدل کاربر - شامل اطلاعات پایه کاربران سیستم
    /// </summary>
    public class User
    {
        /// <summary>
        /// شناسه منحصر به فرد کاربر (Primary Key)
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// نام کاربری (باید یونیک باشد)
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// رمز عبور هش شده کاربر (هرگز رمز عبور ساده ذخیره نشود)
        /// </summary>
        public string PasswordHash { get; set; }

        /// <summary>
        /// نقش کاربر در سیستم (Admin, Operator, Guest)
        /// </summary>
        public UserRole Role { get; set; }
    }

    /// <summary>
    /// نقش‌های مجاز کاربران سیستم
    /// </summary>
    public enum UserRole
    {
        Admin,
        Operator,
        Guest
    }
}