using DeviceDataProcessor.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DeviceDataProcessor.Data
{
    /// <summary>
    /// اینترفیس برای مدیریت داده‌های دستگاه – شامل متدهای کلی و متدهای اختصاصی دستگاه
    /// </summary>
    public interface IDeviceDataRepository : IRepository<DeviceData>
    {
        /// <summary>
        /// دریافت تمامی داده‌های یک دستگاه بر اساس شناسه دستگاه
        /// </summary>
        Task<IEnumerable<DeviceData>> GetByDeviceIdAsync(string deviceId);

        /// <summary>
        /// دریافت آخرین داده ثبت شده از یک دستگاه
        /// </summary>
        Task<DeviceData> GetLatestByDeviceIdAsync(string deviceId);

        /// <summary>
        /// دریافت داده‌ها بر اساس بازه زمانی
        /// </summary>
        Task<IEnumerable<DeviceData>> GetByTimeRangeAsync(string deviceId, DateTime from, DateTime to);

        /// <summary>
        /// دریافت داده‌های دستگاه بر اساس نوع تخلف (SO, OO, ESD)
        /// </summary>
        Task<IEnumerable<DeviceData>> GetByViolationTypeAsync(string deviceId, string violationType);

        /// <summary>
        /// حذف داده‌ها قبل از یک تاریخ مشخص
        /// </summary>
        Task DeleteOldDataAsync(string deviceId, DateTime cutoffDate);
    }
}