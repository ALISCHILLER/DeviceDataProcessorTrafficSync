namespace DeviceDataProcessor.DTOs
{
    // DTO برای دریافت اطلاعات ورود کاربر
    public class LoginRequest
    {
        public string Username { get; set; } // نام کاربری
        public string Password { get; set; } // پسورد
    }
}