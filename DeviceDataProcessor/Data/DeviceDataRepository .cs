using DeviceDataProcessor.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DeviceDataProcessor.Data
{
    // پیاده‌سازی IDeviceDataRepository برای مدیریت داده‌های دستگاه
    public class DeviceDataRepository : IDeviceDataRepository
    {
        private readonly ApplicationDbContext _context; // کانتکست دیتابیس

        public DeviceDataRepository(ApplicationDbContext context)
        {
            _context = context; // دریافت کانتکست
        }

        public async Task<IEnumerable<DeviceData>> GetAllAsync() => await _context.DeviceData.ToListAsync(); // دریافت همه داده‌های دستگاه
        public async Task<DeviceData> GetByIdAsync(int id) => await _context.DeviceData.FindAsync(id); // دریافت داده دستگاه بر اساس شناسه
        public async Task AddAsync(DeviceData deviceData) => await _context.DeviceData.AddAsync(deviceData); // افزودن داده دستگاه
        public async Task UpdateAsync(DeviceData deviceData) => _context.DeviceData.Update(deviceData); // به‌روزرسانی داده دستگاه
        public async Task DeleteAsync(int id)
        {
            var deviceData = await _context.DeviceData.FindAsync(id); // پیدا کردن داده دستگاه بر اساس شناسه
            if (deviceData != null)
                _context.DeviceData.Remove(deviceData); // حذف داده دستگاه
        }

        // متد برای دریافت داده‌ها بر اساس شناسه دستگاه
        public async Task<IEnumerable<DeviceData>> GetByDeviceIdAsync(string deviceId)
        {
            return await _context.DeviceData
                .Where(dd => dd.DeviceId == deviceId)
                .ToListAsync(); // دریافت داده‌های دستگاه بر اساس شناسه دستگاه
        }
    }
}