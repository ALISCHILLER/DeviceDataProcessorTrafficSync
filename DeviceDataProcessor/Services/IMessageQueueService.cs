using System.Threading.Tasks; // برای استفاده از async و await
using DeviceDataProcessor.DTOs; // وارد کردن DTOs برای استفاده در اینترفیس

namespace DeviceDataProcessor.Services
{
    /// <summary>
    /// اینترفیس برای مدیریت صف پیام
    /// </summary>
    public interface IMessageQueueService
    {
        /// <summary>
        /// افزودن داده به صف.
        /// </summary>
        /// <param name="data">داده‌ای که باید به صف اضافه شود.</param>
        /// <returns>یک وظیفه نمایانگر عملیات غیرهمزمان.</returns>
        Task EnqueueAsync(DeviceDataDto data); // افزودن داده به صف

        /// <summary>
        /// دریافت داده از صف.
        /// </summary>
        /// <returns>داده‌ای که از صف دریافت شده است یا null اگر هیچ داده‌ای وجود نداشته باشد.</returns>
        Task<DeviceDataDto> DequeueAsync(); // دریافت داده از صف


        /// <summary>
        /// مقداردهی اولیه برای سرویس صف پیام.
        /// </summary>
        /// <returns>یک وظیفه نمایانگر عملیات غیرهمزمان.</returns>
        Task InitializeAsync(); // متد جدید برای مقداردهی اولیه

    }
}