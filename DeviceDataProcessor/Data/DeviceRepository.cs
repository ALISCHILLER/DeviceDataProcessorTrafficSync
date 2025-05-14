using DeviceDataProcessor.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DeviceDataProcessor.Data
{
    /// <summary>
    /// پیاده‌سازی IRepository<Device> برای مدیریت دستگاه‌ها در دیتابیس
    /// شامل متدهای عمومی + متدهای اختصاصی دستگاه‌های هوشمند ترافیکی
    /// </summary>
    public class DeviceRepository : IRepository<Device>
    {
        private readonly ApplicationDbContext _context;

        public DeviceRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// دریافت تمام دستگاه‌ها همراه با آخرین داده‌های آن‌ها
        /// </summary>
        public async Task<IEnumerable<Device>> GetAllAsync()
            => await _context.Devices
                .Include(d => d.Data)
                .AsNoTracking()
                .ToListAsync();

        /// <summary>
        /// دریافت یک دستگاه بر اساس ID همراه با داده‌ها
        /// </summary>
        public async Task<Device> GetByIdAsync(int id)
            => await _context.Devices
                .Include(d => d.Data)
                .FirstOrDefaultAsync(d => d.Id == id);

        /// <summary>
        /// افزودن یک دستگاه جدید
        /// </summary>
        public async Task AddAsync(Device device)
        {
            await _context.Devices.AddAsync(device);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// به‌روزرسانی اطلاعات یک دستگاه
        /// </summary>
        public async Task UpdateAsync(Device device)
        {
            _context.Devices.Update(device);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// حذف یک دستگاه بر اساس ID
        /// </summary>
        public async Task DeleteAsync(int id)
        {
            var device = await _context.Devices
                .Include(d => d.Data) // اگر داده‌های مرتبط وجود داشت، حذف کنیم؟
                .FirstOrDefaultAsync(d => d.Id == id);

            if (device != null)
            {
                _context.Devices.Remove(device);
                await _context.SaveChangesAsync();
            }
        }

        /// <summary>
        /// دریافت دستگاه بر اساس DeviceId (شناسه منحصر به فرد دستگاه)
        /// </summary>
        public async Task<Device> GetByDeviceIdAsync(string deviceId)
            => await _context.Devices
                .Include(d => d.Data)
                .FirstOrDefaultAsync(d => d.DeviceId == deviceId);

        /// <summary>
        /// دریافت تمام دستگاه‌های آنلاین
        /// </summary>
        public async Task<IEnumerable<Device>> GetOnlineDevicesAsync()
            => await _context.Devices
                .Where(d => d.IsConnected)
                .AsNoTracking()
                .ToListAsync();

        /// <summary>
        /// دریافت تمام دستگاه‌هایی که یک محور مشخص را پوشش می‌دهند
        /// </summary>
        public async Task<IEnumerable<Device>> GetByRoadIdAsync(string roadId)
            => await _context.Devices
                .Where(d => d.RID == roadId || d.GoRoadId == roadId || d.BackRoadId == roadId)
                .ToListAsync();

        /// <summary>
        /// دریافت دستگاه‌های دارای کارت (8 لوپ یا بیشتر)
        /// </summary>
        public async Task<IEnumerable<Device>> GetWithCardAsync()
            => await _context.Devices
                .Where(d => d.CardState == CardState.On)
                .Include(d => d.Data)
                .ToListAsync();

        /// <summary>
        /// دریافت دستگاه‌های بدون کارت (فقط 8 لوپ)
        /// </summary>
        public async Task<IEnumerable<Device>> GetWithoutCardAsync()
            => await _context.Devices
                .Where(d => d.CardState == CardState.Off)
                .Include(d => d.Data)
                .ToListAsync();

        /// <summary>
        /// به‌روزرسانی وضعیت دستگاه (آفلاین -> آنلاین و بالعکس)
        /// </summary>
        public async Task UpdateConnectionStatusAsync(string deviceId, bool isConnected)
        {
            var device = await GetByDeviceIdAsync(deviceId);
            if (device != null)
            {
                device.IsConnected = isConnected;
                device.LastSeen = isConnected ? DateTime.UtcNow : (DateTime?)null;
                await UpdateAsync(device);
            }
        }
    }
}