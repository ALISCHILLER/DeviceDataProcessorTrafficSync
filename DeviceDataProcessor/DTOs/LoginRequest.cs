using System.ComponentModel.DataAnnotations;

namespace DeviceDataProcessor.DTOs
{
    /// <summary>
    /// DTO برای دریافت اطلاعات ورود کاربر از سمت کلاینت
    /// </summary>
    public class LoginRequest
    {
        /// <summary>
        /// نام کاربری - فیلدی الزامی
        /// </summary>
        [Required(ErrorMessage = "وارد کردن نام کاربری الزامی است")]
        [StringLength(100, ErrorMessage = "نام کاربری نباید بیش از 100 کاراکتر باشد")]
        public string Username { get; set; }

        /// <summary>
        /// رمز عبور - فیلدی الزامی
        /// </summary>
        [Required(ErrorMessage = "وارد کردن رمز عبور الزامی است")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
