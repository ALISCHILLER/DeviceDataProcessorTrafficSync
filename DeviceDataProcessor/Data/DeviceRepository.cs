using DeviceDataProcessor.DTOs;
using DeviceDataProcessor.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
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

        // --- CRUD عمومی ---

        public async Task<IEnumerable<Device>> GetAllAsync()
            => await _context.Devices
                .Include(d => d.Data)
                .AsNoTracking()
                .ToListAsync();

        public async Task<Device> GetByIdAsync(int id)
            => await _context.Devices
                .Include(d => d.Data)
                .FirstOrDefaultAsync(d => d.Id == id);

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
            var device = await _context.Devices
                .Include(d => d.Data)
                .FirstOrDefaultAsync(d => d.Id == id);

            if (device != null)
            {
                _context.Devices.Remove(device);
                await _context.SaveChangesAsync();
            }
        }

        // --- متدهای اختصاصی دستگاه ---

        public async Task<Device> GetByDeviceIdAsync(string deviceId)
            => await _context.Devices
                .Include(d => d.Data)
                .FirstOrDefaultAsync(d => d.DeviceId == deviceId);

        public async Task<IEnumerable<Device>> GetOnlineDevicesAsync()
            => await _context.Devices
                .Where(d => d.IsConnected)
                .AsNoTracking()
                .ToListAsync();

        public async Task<IEnumerable<Device>> GetByRoadIdAsync(string roadId)
            => await _context.Devices
                .Where(d => d.RID == roadId || d.GoRoadId == roadId || d.BackRoadId == roadId)
                .Include(d => d.Data)
                .ToListAsync();

        public async Task<IEnumerable<Device>> GetWithCardAsync()
            => await _context.Devices
                .Where(d => d.CardState == CardState.On)
                .Include(d => d.Data)
                .ToListAsync();

        public async Task<IEnumerable<Device>> GetWithoutCardAsync()
            => await _context.Devices
                .Where(d => d.CardState == CardState.Off)
                .Include(d => d.Data)
                .ToListAsync();

        public async Task UpdateConnectionStatusAsync(string deviceId, bool isConnected)
        {
            var device = await GetByDeviceIdAsync(deviceId);
            if (device != null)
            {
                device.IsConnected = isConnected;
                device.LastUpdated = DateTime.UtcNow;
                device.LastSeen = isConnected ? DateTime.UtcNow : (DateTime?)null;
                await UpdateAsync(device);
            }
        }

        // --- متدهای داده‌های دستگاه ---

        public async Task<DeviceData> GetLatestDataByDeviceIdAsync(string deviceId)
            => await _context.DeviceData
                .Where(d => d.DeviceId == deviceId)
                .OrderByDescending(d => d.Timestamp)
                .FirstOrDefaultAsync();

        public async Task<IEnumerable<DeviceData>> GetDataByTimeRangeAsync(string deviceId, DateTime from, DateTime to)
            => await _context.DeviceData
                .Where(d => d.DeviceId == deviceId && d.Timestamp >= from && d.Timestamp <= to)
                .ToListAsync();

        public async Task<IEnumerable<DeviceData>> GetViolationDataAsync(string deviceId, string violationType)
        {
            var query = _context.DeviceData.Where(d => d.DeviceId == deviceId);

            return violationType.ToLower() switch
            {
                "speed" => await query.Where(d => d.SO > 0).ToListAsync(),
                "overtaking" => await query.Where(d => d.OO > 0).ToListAsync(),
                "distance" => await query.Where(d => d.ESD > 0).ToListAsync(),
                _ => await query.ToListAsync()
            };
        }

        public async Task<bool> DeleteOldDataAsync(string deviceId, DateTime cutoffDate)
        {
            var oldData = await _context.DeviceData
                .Where(d => d.DeviceId == deviceId && d.Timestamp < cutoffDate)
                .ToListAsync();

            if (!oldData.Any()) return false;

            _context.DeviceData.RemoveRange(oldData);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> IsDeviceConnectedAsync(string deviceId)
            => await _context.Devices
                .AnyAsync(d => d.DeviceId == deviceId && d.IsConnected);

        public async Task UpdateLastSeenAsync(string deviceId, DateTime timestamp)
        {
            var device = await GetByDeviceIdAsync(deviceId);
            if (device != null)
            {
                device.LastSeen = timestamp;
                await UpdateAsync(device);
            }
        }

        // --- تنظیمات دستگاه ---

        public async Task<DeviceSettingsDto> GetSettingsByDeviceIdAsync(string deviceId)
        {
            var device = await GetByDeviceIdAsync(deviceId);
            if (device == null) return null;

            return new DeviceSettingsDto
            {
                Name = device.Name,
                Location = device.Location,
                Status = device.Status.ToString(),
                FID = device.FID,
                RID = device.RID,

                LoopLengths = device.LoopLengths,
                LoopDistances = device.LoopDistances,
                LoopOffsets = device.LoopOffsets,
                DistanceOffsets = device.DistanceOffsets,

                GoRoadId = device.GoRoadId,
                BackRoadId = device.BackRoadId,
                GoLaneCount = device.GoLaneCount,
                BackLaneCount = device.BackLaneCount,

                Class1Length = device.Class1Length,
                Class2Length = device.Class2Length,
                Class3Length = device.Class3Length,
                Class4Length = device.Class4Length,

                LightVehicleSpeedDay = device.LightVehicleSpeedDay,
                LightVehicleSpeedNight = device.LightVehicleSpeedNight,
                HeavyVehicleSpeedDay = device.HeavyVehicleSpeedDay,
                HeavyVehicleSpeedNight = device.HeavyVehicleSpeedNight,

                DayStartTime = device.DayStartTime,
                DayEndTime = device.DayEndTime,
                GapTime = device.GapTime,

                Ip1 = device.Ip1,
                Ip2 = device.Ip2,
                Ip3 = device.Ip3,
                Port = device.Port,
                SummaryTime = device.SummaryTime,
                PhoneNumber1 = device.PhoneNumber1,
                PhoneNumber2 = device.PhoneNumber2,

                CardState = device.CardState,
                CardDirection = device.CardDirection,
                CardLoopLengths = device.CardLoopLengths
            };
        }

        public async Task<bool> UpdateSettingsAsync(string deviceId, DeviceSettingsDto settings)
        {
            var device = await GetByDeviceIdAsync(deviceId);
            if (device == null) return false;

            // --- بروزرسانی تنظیمات عمومی ---
            device.Name = settings.Name;
            device.Location = settings.Location;
            device.Status = Enum.Parse<DeviceStatus>(settings.Status);
            device.FID = settings.FID;
            device.RID = settings.RID;

            // --- بروزرسانی لوپ‌ها ---
            device.LoopLengths = settings.LoopLengths;
            device.LoopDistances = settings.LoopDistances;
            device.LoopOffsets = settings.LoopOffsets;
            device.DistanceOffsets = settings.DistanceOffsets;

            // --- بروزرسانی محور ---
            device.GoRoadId = settings.GoRoadId;
            device.BackRoadId = settings.BackRoadId;
            device.GoLaneCount = settings.GoLaneCount;
            device.BackLaneCount = settings.BackLaneCount;

            // --- بروزرسانی طول کلاس خودروها ---
            device.Class1Length = settings.Class1Length;
            device.Class2Length = settings.Class2Length;
            device.Class3Length = settings.Class3Length;
            device.Class4Length = settings.Class4Length;

            // --- بروزرسانی سرعت مجاز ---
            device.LightVehicleSpeedDay = settings.LightVehicleSpeedDay;
            device.LightVehicleSpeedNight = settings.LightVehicleSpeedNight;
            device.HeavyVehicleSpeedDay = settings.HeavyVehicleSpeedDay;
            device.HeavyVehicleSpeedNight = settings.HeavyVehicleSpeedNight;
            device.DayStartTime = settings.DayStartTime;
            device.DayEndTime = settings.DayEndTime;
            device.GapTime = settings.GapTime;

            // --- بروزرسانی شبکه ---
            device.Ip1 = settings.Ip1;
            device.Ip2 = settings.Ip2;
            device.Ip3 = settings.Ip3;
            device.Port = settings.Port;
            device.SummaryTime = settings.SummaryTime;
            device.PhoneNumber1 = settings.PhoneNumber1;
            device.PhoneNumber2 = settings.PhoneNumber2;

            // --- بروزرسانی کارت ---
            device.CardState = settings.CardState;
            device.CardDirection = settings.CardDirection;
            device.CardLoopLengths = settings.CardLoopLengths;

            await UpdateAsync(device);
            return true;
        }
    }
}