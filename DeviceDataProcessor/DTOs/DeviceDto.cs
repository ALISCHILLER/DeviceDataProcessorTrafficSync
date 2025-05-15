using DeviceDataProcessor.Models;
using System;

namespace DeviceDataProcessor.DTOs
{
    /// <summary>
    /// DTO برای افزودن و ویرایش دستگاه
    /// </summary>
    public class DeviceDto
    {
        /// <summary>
        /// نام دستگاه (اسم محور)
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// شناسه اولیه دستگاه (FID)
        /// </summary>
        public string FID { get; set; }

        /// <summary>
        /// شناسه محور دستگاه (RID)
        /// </summary>
        public string RID { get; set; }

        /// <summary>
        /// شناسه منحصر به فرد دستگاه (مثل شماره سریال)
        /// </summary>
        public string DeviceId { get; set; }

        /// <summary>
        /// موقعیت جغرافیایی دستگاه (مثلاً "محور تهران - قم")
        /// </summary>
        public string Location { get; set; }


        /// <summary>
        /// وضعیت دستگاه – مشخص کننده اتصال و فعالیت دستگاه
        /// </summary>
        public DeviceStatus Status { get; set; }
    }
}