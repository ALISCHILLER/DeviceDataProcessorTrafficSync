namespace DeviceDataProcessor.DTOs
{
    // DTO برای ثبت نام کاربر جدید
    public class RegisterRequest
    {
        public string Username { get; set; } // نام کاربری
        public string Password { get; set; } // پسورد
        public string Role { get; set; } // نقش کاربر
    }
}