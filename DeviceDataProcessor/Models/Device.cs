namespace DeviceDataProcessor.Models
{
    // مدل دستگاه که اطلاعات مربوط به دستگاه‌ها را ذخیره می‌کند
    public class Device
    {
        public int Id { get; set; } // شناسه دستگاه
        public string DeviceId { get; set; } // شناسه منحصر به فرد دستگاه
        public string Name { get; set; } // نام دستگاه
        public string Location { get; set; } // موقعیت دستگاه
        public string Status { get; set; } // وضعیت دستگاه (فعال، غیرفعال و ...)
        public DateTime LastUpdated { get; set; } // زمان آخرین به‌روزرسانی
    }
}