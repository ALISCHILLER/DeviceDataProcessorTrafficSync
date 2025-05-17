using DeviceDataProcessor.DTOs;
using DeviceDataProcessor.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DeviceDataProcessor.Services
{
    /// <summary>
    /// اینترفیس خدمات پردازش داده‌های دستگاه – شامل متدهای لازم برای مدیریت دستگاه‌ها و داده‌های آن‌ها
    /// </summary>
    public interface IDataProcessorService
    {
        // --- داده‌های دستگاه ---

        /// <summary>
        /// پردازش و ذخیره داده دریافتی از دستگاه (MQTT / API)
        /// </summary>
        Task<bool> ProcessDataAsync(DeviceDataDto data);

        /// <summary>
        /// دریافت تمام داده‌های یک دستگاه
        /// </summary>
        Task<IEnumerable<DeviceData>> GetDeviceDataAsync(string deviceId);

        /// <summary>
        /// دریافت آخرین داده ثبت شده از یک دستگاه
        /// </summary>
        Task<DeviceData> GetLatestDeviceDataAsync(string deviceId);

        /// <summary>
        /// دریافت داده‌ها بر اساس بازه زمانی
        /// </summary>
        Task<IEnumerable<DeviceData>> GetDeviceDataByTimeRangeAsync(string deviceId, DateTime from, DateTime to);

        /// <summary>
        /// دریافت داده‌های دارای تخلف (Speeding, Overtaking, SafeDistance)
        /// </summary>
        Task<IEnumerable<DeviceData>> GetViolationDataAsync(string deviceId, string violationType);

        // --- تنظیمات دستگاه ---

        /// <summary>
        /// دریافت لیست تمام دستگاه‌ها
        /// </summary>
        Task<IEnumerable<DeviceDto>> GetAllDevicesAsync();

        /// <summary>
        /// دریافت اطلاعات یک دستگاه بر اساس DeviceId
        /// </summary>
        Task<DeviceDto> GetDeviceByIdAsync(string deviceId);

        /// <summary>
        /// دریافت تنظیمات کامل یک دستگاه
        /// </summary>
        Task<DeviceSettingsDto> GetSettingsByDeviceIdAsync(string deviceId);

        /// <summary>
        /// به‌روزرسانی تنظیمات کامل یک دستگاه
        /// </summary>
        Task<bool> UpdateSettingsAsync(string deviceId, DeviceSettingsDto settings);

        /// <summary>
        /// حذف داده‌های قدیمی‌تر از یک تاریخ مشخص
        /// </summary>
        Task<bool> DeleteOldDataAsync(string deviceId, DateTime cutoffDate);

        /// <summary>
        /// به‌روزرسانی وضعیت دستگاه (Online/Offline) و زمان آخرین دیده شدن
        /// </summary>
        Task<bool> UpdateConnectionStatusAsync(string deviceId, bool isConnected);


        Task<IEnumerable<DeviceDataDto>> GetDataByTimeRangeAsync(string deviceId, DateTime from, DateTime to);


        Task<bool> UpdateDeviceAsync(string deviceId, DeviceSettingsDto updateDto);
    }
}