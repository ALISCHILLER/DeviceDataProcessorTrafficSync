using DeviceDataProcessor.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DeviceDataProcessor.Data
{
    /// <summary>
    /// پیاده‌سازی IDeviceDataRepository برای مدیریت داده‌های دستگاه
    /// </summary>
    public class DeviceDataRepository : IDeviceDataRepository
    {
        private readonly ApplicationDbContext _context; // کانتکست EF Core

        public DeviceDataRepository(ApplicationDbContext context)
        {
            _context = context; // دریافت کانتکست از طریق DI
        }

        /// <summary>
        /// دریافت تمامی داده‌ها
        /// </summary>
        public async Task<IEnumerable<DeviceData>> GetAllAsync()
            => await _context.DeviceData
                .AsNoTracking()
                .ToListAsync();

        /// <summary>
        /// دریافت داده بر اساس ID
        /// </summary>
        public async Task<DeviceData> GetByIdAsync(int id)
            => await _context.DeviceData.FindAsync(id);

        /// <summary>
        /// افزودن داده جدید
        /// </summary>
        public async Task AddAsync(DeviceData deviceData)
        {
            await _context.DeviceData.AddAsync(deviceData);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// به‌روزرسانی داده
        /// </summary>
        public async Task UpdateAsync(DeviceData deviceData)
        {
            _context.DeviceData.Update(deviceData);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// حذف داده بر اساس ID
        /// </summary>
        public async Task DeleteAsync(int id)
        {
            var data = await _context.DeviceData.FindAsync(id);
            if (data != null)
            {
                _context.DeviceData.Remove(data);
                await _context.SaveChangesAsync();
            }
        }

        /// <summary>
        /// دریافت تمام داده‌های یک دستگاه بر اساس DeviceId
        /// </summary>
        public async Task<IEnumerable<DeviceData>> GetByDeviceIdAsync(string deviceId)
        {
            return await _context.DeviceData
                .Where(dd => dd.DeviceId == deviceId)
                .OrderByDescending(dd => dd.Timestamp)
                .ToListAsync();
        }

        /// <summary>
        /// دریافت آخرین داده ثبت شده از یک دستگاه
        /// </summary>
        public async Task<DeviceData> GetLatestByDeviceIdAsync(string deviceId)
        {
            return await _context.DeviceData
                .Where(dd => dd.DeviceId == deviceId)
                .OrderByDescending(dd => dd.Timestamp)
                .FirstOrDefaultAsync();
        }

        /// <summary>
        /// دریافت داده‌ها بر اساس بازه زمانی
        /// </summary>
        public async Task<IEnumerable<DeviceData>> GetByTimeRangeAsync(string deviceId, DateTime from, DateTime to)
        {
            return await _context.DeviceData
                .Where(dd => dd.DeviceId == deviceId && dd.Timestamp >= from && dd.Timestamp <= to)
                .ToListAsync();
        }

        /// <summary>
        /// دریافت داده‌ها بر اساس نوع تخلف
        /// </summary>
        public async Task<IEnumerable<DeviceData>> GetByViolationTypeAsync(string deviceId, string violationType)
        {
            var query = _context.DeviceData.Where(d => d.DeviceId == deviceId);

            switch (violationType.ToLower())
            {
                case "speed":
                    return await query.Where(d => d.SO > 0).ToListAsync();
                case "overtaking":
                    return await query.Where(d => d.OO > 0).ToListAsync();
                case "distance":
                    return await query.Where(d => d.ESD > 0).ToListAsync();
                default:
                    return await query.ToListAsync();
            }
        }

        /// <summary>
        /// حذف داده‌های قدیمی‌تر از یک تاریخ مشخص
        /// </summary>
        public async Task DeleteOldDataAsync(string deviceId, DateTime cutoffDate)
        {
            var oldData = await _context.DeviceData
                .Where(d => d.DeviceId == deviceId && d.Timestamp < cutoffDate)
                .ToListAsync();

            if (oldData.Count > 0)
            {
                _context.DeviceData.RemoveRange(oldData);
                await _context.SaveChangesAsync();
            }
        }
    }
}