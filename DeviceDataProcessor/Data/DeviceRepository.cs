using DeviceDataProcessor.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DeviceDataProcessor.Data
{
    // پیاده‌سازی IRepository برای مدیریت دستگاه‌ها
    public class DeviceRepository : IRepository<Device>
    {
        private readonly ApplicationDbContext _context; // کانتکست دیتابیس

        public DeviceRepository(ApplicationDbContext context)
        {
            _context = context; // دریافت کانتکست
        }

        public async Task<IEnumerable<Device>> GetAllAsync() => await _context.Devices.ToListAsync(); // دریافت همه دستگاه‌ها
        public async Task<Device> GetByIdAsync(int id) => await _context.Devices.FindAsync(id); // دریافت دستگاه بر اساس شناسه
        public async Task AddAsync(Device device) => await _context.Devices.AddAsync(device); // افزودن دستگاه
        public async Task UpdateAsync(Device device) => _context.Devices.Update(device); // به‌روزرسانی دستگاه
        public async Task DeleteAsync(int id)
        {
            var device = await _context.Devices.FindAsync(id); // پیدا کردن دستگاه بر اساس شناسه
            if (device != null)
                _context.Devices.Remove(device); // حذف دستگاه
        }
    }
}