using DeviceDataProcessor.Models;
using System.Collections.Generic;

namespace DeviceDataProcessor.DTOs
{
    /// <summary>
    /// DTO برای به‌روزرسانی اطلاعات دستگاه هوشمند ترافیکی
    /// </summary>
    public class DeviceUpdateDto
    {
        /// <summary>
        /// نام دستگاه – اسم محور یا محل قرارگیری دستگاه
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// موقعیت دستگاه – مثل "تهران - شمال"
        /// </summary>
        public string Location { get; set; }

        /// <summary>
        /// وضعیت دستگاه – Online یا Offline
        /// </summary>
        public DeviceStatus? Status { get; set; }

        /// <summary>
        /// FIRST ID دستگاه
        /// </summary>
        public string FID { get; set; }

        /// <summary>
        /// ROAD ID دستگاه
        /// </summary>
        public string RID { get; set; }
    }
}