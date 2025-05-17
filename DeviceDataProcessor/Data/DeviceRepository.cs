using DeviceDataProcessor.DTOs;
using DeviceDataProcessor.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DeviceDataProcessor.Data
{
    public class DeviceRepository : IDeviceRepository
    {
        private readonly ApplicationDbContext _context;

        public DeviceRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        #region General CRUD

        /// <summary>
        /// دریافت تمام دستگاه‌ها به همراه داده‌های مرتبط
        /// </summary>
        public async Task<IEnumerable<Device>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await _context.Devices
                .Include(d => d.Data)
                .AsNoTracking()
                .ToListAsync(cancellationToken);
        }

        /// <summary>
        /// دریافت دستگاه با شناسه داخلی
        /// </summary>
        public async Task<Device> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _context.Devices
                .Include(d => d.Data)
                .FirstOrDefaultAsync(d => d.Id == id, cancellationToken);
        }

        /// <summary>
        /// افزودن دستگاه جدید
        /// </summary>
        public async Task AddAsync(Device device, CancellationToken cancellationToken = default)
        {
            await _context.Devices.AddAsync(device, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }

        /// <summary>
        /// به‌روزرسانی دستگاه
        /// </summary>
        public async Task UpdateAsync(Device device, CancellationToken cancellationToken = default)
        {
            _context.Devices.Update(device);
            await _context.SaveChangesAsync(cancellationToken);
        }

        /// <summary>
        /// حذف دستگاه بر اساس شناسه داخلی
        /// </summary>
        public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
        {
            var device = await _context.Devices.FindAsync(new object[] { id }, cancellationToken);
            if (device != null)
            {
                _context.Devices.Remove(device);
                await _context.SaveChangesAsync(cancellationToken);
            }
        }

        #endregion

        #region Device Specific

        /// <summary>
        /// دریافت دستگاه بر اساس DeviceId
        /// </summary>
        public async Task<Device> GetByDeviceIdAsync(string deviceId, CancellationToken cancellationToken = default)
        {
            return await _context.Devices
                .Include(d => d.Data)
                .FirstOrDefaultAsync(d => d.DeviceId == deviceId, cancellationToken);
        }

        /// <summary>
        /// بررسی اتصال دستگاه
        /// </summary>
        public async Task<bool> IsDeviceConnectedAsync(string deviceId, CancellationToken cancellationToken = default)
        {
            return await _context.Devices
                .AnyAsync(d => d.DeviceId == deviceId && d.IsConnected, cancellationToken);
        }

        /// <summary>
        /// بروزرسانی زمان آخرین مشاهده دستگاه
        /// </summary>
        public async Task UpdateLastSeenAsync(string deviceId, DateTime timestamp, CancellationToken cancellationToken = default)
        {
            var device = await GetByDeviceIdAsync(deviceId, cancellationToken);
            if (device != null)
            {
                device.LastSeen = timestamp;
                _context.Devices.Update(device);
                await _context.SaveChangesAsync(cancellationToken);
            }
        }

        /// <summary>
        /// دریافت آخرین داده ثبت شده از دستگاه
        /// </summary>
        public async Task<DeviceData> GetLatestDataByDeviceIdAsync(string deviceId, CancellationToken cancellationToken = default)
        {
            return await _context.DeviceData
                .Where(d => d.DeviceId == deviceId)
                .OrderByDescending(d => d.Timestamp)
                .FirstOrDefaultAsync(cancellationToken);
        }

        /// <summary>
        /// دریافت داده‌های دستگاه در بازه زمانی مشخص
        /// </summary>
        public async Task<IEnumerable<DeviceData>> GetDataByTimeRangeAsync(string deviceId, DateTime from, DateTime to, CancellationToken cancellationToken = default)
        {
            return await _context.DeviceData
                .Where(d => d.DeviceId == deviceId && d.Timestamp >= from && d.Timestamp <= to)
                .ToListAsync(cancellationToken);
        }

        /// <summary>
        /// دریافت داده‌های نقض قوانین (Violation Data)
        /// </summary>
        /// <param name="violationType">نوع نقض: speed, overtaking, distance</param>
        public async Task<IEnumerable<DeviceData>> GetViolationDataAsync(string deviceId, string violationType, CancellationToken cancellationToken = default)
        {
            IQueryable<DeviceData> query = _context.DeviceData.Where(d => d.DeviceId == deviceId);

            switch (violationType.ToLowerInvariant())
            {
                case "speed":
                    query = query.Where(d => d.SO > 0);
                    break;
                case "overtaking":
                    query = query.Where(d => d.OO > 0);
                    break;
                case "distance":
                    query = query.Where(d => d.ESD > 0);
                    break;
                default:
                    // اگر نوع نامشخص بود، همه داده‌ها را برگردان
                    break;
            }

            return await query.ToListAsync(cancellationToken);
        }

        /// <summary>
        /// حذف داده‌های قدیمی دستگاه بر اساس تاریخ قطع شده
        /// </summary>
        public async Task<bool> DeleteOldDataAsync(string deviceId, DateTime cutoffDate, CancellationToken cancellationToken = default)
        {
            var oldData = await _context.DeviceData
                .Where(d => d.DeviceId == deviceId && d.Timestamp < cutoffDate)
                .ToListAsync(cancellationToken);

            if (!oldData.Any())
                return false;

            _context.DeviceData.RemoveRange(oldData);
            await _context.SaveChangesAsync(cancellationToken);

            return true;
        }

        /// <summary>
        /// بروزرسانی تنظیمات دستگاه
        /// </summary>
        public async Task<bool> UpdateSettingsAsync(string deviceId, DeviceSettingsDto settings, CancellationToken cancellationToken = default)
        {
            var device = await GetByDeviceIdAsync(deviceId, cancellationToken);
            if (device == null)
                return false;

            // بروز رسانی مقادیر دستگاه از DTO
            device.Name = settings.Name;
            device.Location = settings.Location;

            if (Enum.TryParse<DeviceStatus>(settings.Status, true, out var status))
                device.Status = status;

            device.FID = settings.FID;
            device.RID = settings.RID;

            device.LoopLengths = settings.LoopLengths;
            device.LoopDistances = settings.LoopDistances;
            device.LoopOffsets = settings.LoopOffsets;
            device.DistanceOffsets = settings.DistanceOffsets;

            device.GoRoadId = settings.GoRoadId;
            device.BackRoadId = settings.BackRoadId;
            device.GoLaneCount = settings.GoLaneCount;
            device.BackLaneCount = settings.BackLaneCount;

            device.Class1Length = settings.Class1Length;
            device.Class2Length = settings.Class2Length;
            device.Class3Length = settings.Class3Length;
            device.Class4Length = settings.Class4Length;

            device.LightVehicleSpeedDay = settings.LightVehicleSpeedDay;
            device.LightVehicleSpeedNight = settings.LightVehicleSpeedNight;
            device.HeavyVehicleSpeedDay = settings.HeavyVehicleSpeedDay;
            device.HeavyVehicleSpeedNight = settings.HeavyVehicleSpeedNight;
            device.DayStartTime = settings.DayStartTime;
            device.DayEndTime = settings.DayEndTime;
            device.GapTime = settings.GapTime;

            device.Ip1 = settings.Ip1;
            device.Ip2 = settings.Ip2;
            device.Ip3 = settings.Ip3;
            device.Port = settings.Port;
            device.SummaryTime = settings.SummaryTime;
            device.PhoneNumber1 = settings.PhoneNumber1;
            device.PhoneNumber2 = settings.PhoneNumber2;

            device.CardState = settings.CardState;
            device.CardDirection = settings.CardDirection;
            device.CardLoopLengths = settings.CardLoopLengths;

            _context.Devices.Update(device);
            await _context.SaveChangesAsync(cancellationToken);

            return true;
        }

        #endregion
    }
}
