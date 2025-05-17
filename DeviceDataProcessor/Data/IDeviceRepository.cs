using DeviceDataProcessor.DTOs;
using DeviceDataProcessor.Models;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace DeviceDataProcessor.Data
{
    /// <summary>
    /// اینترفیس مخزن دستگاه‌ها
    /// شامل تمام متدهای لازم برای مدیریت دستگاه و داده‌های مرتبط با آن
    /// اینترفیس از الگوی Repository پیروی می‌کند و پشتیبانی از عملیات async و CancellationToken دارد.
    /// </summary>
    public interface IDeviceRepository
    {
        Task<Device> GetByDeviceIdAsync(string deviceId, CancellationToken cancellationToken = default);
        Task<bool> IsDeviceConnectedAsync(string deviceId, CancellationToken cancellationToken = default);
        Task<DeviceData> GetLatestDataByDeviceIdAsync(string deviceId, CancellationToken cancellationToken = default);
        Task<IEnumerable<DeviceData>> GetDataByTimeRangeAsync(string deviceId, DateTime from, DateTime to, CancellationToken cancellationToken = default);
        Task<IEnumerable<DeviceData>> GetViolationDataAsync(string deviceId, string violationType, CancellationToken cancellationToken = default);
        Task<bool> DeleteOldDataAsync(string deviceId, DateTime cutoffDate, CancellationToken cancellationToken = default);
        Task UpdateLastSeenAsync(string deviceId, DateTime timestamp, CancellationToken cancellationToken = default);
        Task<bool> UpdateSettingsAsync(string deviceId, DeviceSettingsDto settings, CancellationToken cancellationToken = default);

        // Optional general CRUD
        Task<IEnumerable<Device>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<Device> GetByIdAsync(int id, CancellationToken cancellationToken = default);
        Task AddAsync(Device device, CancellationToken cancellationToken = default);
        Task UpdateAsync(Device device, CancellationToken cancellationToken = default);
        Task DeleteAsync(int id, CancellationToken cancellationToken = default);
    }
}
