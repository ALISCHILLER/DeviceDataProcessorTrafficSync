namespace DeviceDataProcessor.Models
{
    // مدل پاسخ API که اطلاعات مربوط به پاسخ‌های دریافتی از سرور خارجی را ذخیره می‌کند
    public class ApiResponse
    {
        public int Id { get; set; } // شناسه پاسخ API
        public string DeviceId { get; set; } // شناسه دستگاه
        public DateTime Timestamp { get; set; } // زمان ثبت پاسخ
        public string Response { get; set; } // محتوای پاسخ
    }
}