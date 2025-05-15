using DeviceDataProcessor.DTOs;
using DeviceDataProcessor.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DeviceDataProcessor.Data
{
    /// <summary>
    /// اینترفیس مخزن دستگاه – شامل تمام متدهای لازم برای مدیریت دستگاه و تنظیمات آن
    /// </summary>
    public interface IDeviceRepository : IRepository<Device>
    {
        // --- عمومی ---

        /// <summary>
        /// دریافت دستگاه بر اساس شناسه منحصر به فرد (DeviceId)
        /// </summary>
        Task<Device> GetByDeviceIdAsync(string deviceId);

        /// <summary>
        /// چک کردن اتصال دستگاه به سرور
        /// </summary>
        Task<bool> IsDeviceConnectedAsync(string deviceId);

        /// <summary>
        /// بروزرسانی زمان آخرین داده دریافتی (LastSeen)
        /// </summary>
        Task UpdateLastSeenAsync(string deviceId, DateTime timestamp);

        // --- داده‌های دستگاه ---

        /// <summary>
        /// دریافت آخرین داده ثبت شده از دستگاه
        /// </summary>
        Task<DeviceData> GetLatestDataByDeviceIdAsync(string deviceId);

        /// <summary>
        /// دریافت داده‌های دستگاه بر اساس بازه زمانی
        /// </summary>
        Task<IEnumerable<DeviceData>> GetDataByTimeRangeAsync(string deviceId, DateTime from, DateTime to);

        /// <summary>
        /// دریافت داده‌های دارای تخلف (Speeding, Overtaking, SafeDistance)
        /// </summary>
        Task<IEnumerable<DeviceData>> GetViolationDataAsync(string deviceId, string violationType);

        /// <summary>
        /// حذف داده‌های قدیمی‌تر از یک تاریخ مشخص
        /// </summary>
        Task<bool> DeleteOldDataAsync(string deviceId, DateTime cutoffDate);

        // --- تنظیمات دستگاه ---

        /// <summary>
        /// دریافت تنظیمات کامل دستگاه
        /// </summary>
        Task<DeviceSettingsDto> GetSettingsByDeviceIdAsync(string deviceId);

        /// <summary>
        /// به‌روزرسانی تنظیمات دستگاه
        /// </summary>
        Task<bool> UpdateSettingsAsync(string deviceId, DeviceSettingsDto settings);
    }
}