using DeviceDataProcessor.Models;
using System.ComponentModel.DataAnnotations;

namespace DeviceDataProcessor.DTOs
{
    /// <summary>
    /// DTO برای دریافت اطلاعات ثبت‌نام کاربر جدید
    /// </summary>
    public class RegisterRequest
    {
        /// <summary>
        /// نام کاربری - الزامی و دارای حداکثر طول 100 کاراکتر
        /// </summary>
        [Required(ErrorMessage = "وارد کردن نام کاربری الزامی است")]
        [StringLength(100, ErrorMessage = "نام کاربری نباید بیش از 100 کاراکتر باشد")]
        public string Username { get; set; } = string.Empty;

        /// <summary>
        /// رمز عبور - الزامی و دارای حداقل و حداکثر طول
        /// </summary>
        [Required(ErrorMessage = "وارد کردن رمز عبور الزامی است")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "رمز عبور باید حداقل 6 کاراکتر باشد")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;

        /// <summary>
        /// نقش کاربر - Admin یا Operator یا Guest
        /// </summary>
        [Required(ErrorMessage = "وارد کردن نقش کاربر الزامی است")]
        public UserRole Role { get; set; }
    }
}
