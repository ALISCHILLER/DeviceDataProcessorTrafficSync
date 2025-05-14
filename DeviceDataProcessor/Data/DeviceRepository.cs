using DeviceDataProcessor.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DeviceDataProcessor.Data
{
    /// <summary>
    /// پیاده‌سازی IRepository<Device> برای مدیریت دستگاه‌ها در دیتابیس
    /// </summary>
    public class DeviceRepository : IRepository<Device>
    {
        private readonly ApplicationDbContext _context;

        public DeviceRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Device>> GetAllAsync() =>
            await _context.Devices.Include(d => d.Data).AsNoTracking().ToListAsync();

        public async Task<Device> GetByIdAsync(int id) =>
            await _context.Devices.Include(d => d.Data).FirstOrDefaultAsync(d => d.Id == id);

        public async Task AddAsync(Device device)
        {
            await _context.Devices.AddAsync(device);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Device device)
        {
            _context.Devices.Update(device);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var device = await _context.Devices.FindAsync(id);
            if (device != null)
            {
                _context.Devices.Remove(device);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<Device> GetByDeviceIdAsync(string deviceId) =>
            await _context.Devices.Include(d => d.Data).FirstOrDefaultAsync(d => d.DeviceId == deviceId);

        public async Task<IEnumerable<Device>> GetOnlineDevicesAsync() =>
            await _context.Devices.Where(d => d.IsConnected).ToListAsync();

   
    }
}