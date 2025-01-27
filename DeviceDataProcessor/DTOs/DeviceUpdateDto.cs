namespace DeviceDataProcessor.DTOs
{
    // DTO برای به‌روزرسانی اطلاعات دستگاه
    public class DeviceUpdateDto
    {
        public string Name { get; set; } // نام دستگاه
        public string Location { get; set; } // موقعیت دستگاه
        public string Status { get; set; } // وضعیت دستگاه
    }
}