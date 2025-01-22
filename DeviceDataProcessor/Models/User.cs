namespace DeviceDataProcessor.Models
{
    public class User
    {
        public int Id { get; set; } // شناسه کاربر
        public string Username { get; set; } // نام کاربری
        public string PasswordHash { get; set; } // هش پسورد
        public string Role { get; set; } // نقش کاربر
    }
}