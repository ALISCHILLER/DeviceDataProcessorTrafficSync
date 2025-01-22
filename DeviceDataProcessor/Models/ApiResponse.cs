namespace DeviceDataProcessor.Models
{
    public class ApiResponse
    {
        public int Id { get; set; } // شناسه پاسخ API
        public string DeviceId { get; set; } // شناسه دستگاه
        public DateTime Timestamp { get; set; } // زمان ثبت پاسخ
        public string Response { get; set; } // محتوای پاسخ
    }
}