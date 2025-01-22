namespace DeviceDataProcessor.Models
{
    public class Device
    {
        public int Id { get; set; } // شناسه دستگاه
        public string DeviceId { get; set; } // شناسه منحصر به فرد دستگاه
        public string Name { get; set; } // نام دستگاه
        public string Location { get; set; } // موقعیت دستگاه
    }
}