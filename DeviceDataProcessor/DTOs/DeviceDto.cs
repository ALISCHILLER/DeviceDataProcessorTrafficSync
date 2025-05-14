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
    }
}