using DeviceDataProcessor.DTOs;
using DeviceDataProcessor.Models;
using DeviceDataProcessor.Data;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DeviceDataProcessor.Services
{
    /// <summary>
    /// پیاده‌سازی IDataProcessorService برای مدیریت داده‌های دستگاه هوشمند ترافیکی
    /// </summary>
    public class DataProcessorService : IDataProcessorService
    {
        private readonly IDeviceDataRepository _deviceDataRepository; // مخزن داده‌های دستگاه
        private readonly IDeviceRepository _deviceRepository;       // مخزن دستگاه‌ها
        private readonly IMessageQueueService _messageQueueService; // صف داده (MQTT / Redis)
        private readonly RedisService _redisService;               // کش داده

        public DataProcessorService(
            IDeviceDataRepository deviceDataRepository,
            IDeviceRepository deviceRepository,
            IMessageQueueService messageQueueService,
            RedisService redisService)
        {
            _deviceDataRepository = deviceDataRepository;
            _deviceRepository = deviceRepository;
            _messageQueueService = messageQueueService;
            _redisService = redisService;
        }

        // --- داده‌های دستگاه ---

        /// <summary>
        /// پردازش و ذخیره داده دریافتی از دستگاه (مانند MQTT, API)
        /// </summary>
        public async Task<bool> ProcessDataAsync(DeviceDataDto data)
        {
            if (data == null || string.IsNullOrEmpty(data.DeviceId))
                return false;

            var deviceExists = await _deviceRepository.GetByDeviceIdAsync(data.DeviceId);
            if (deviceExists == null)
                return false;

            var entity = new DeviceData
            {
                DeviceId = data.DeviceId,
                Timestamp = data.Timestamp,
                ST = data.ST,
                ET = data.ET,
                C1 = data.C1,
                C2 = data.C2,
                C3 = data.C3,
                C4 = data.C4,
                C5 = data.C5,
                ASP = data.ASP ?? 0,
                SO = data.SO ?? 0,
                OO = data.OO ?? 0,
                ESD = data.ESD ?? 0
            };

            await _deviceDataRepository.AddAsync(entity);
            await _messageQueueService.EnqueueAsync(entity);
            await _redisService.SetAsync(data.DeviceId, entity, TimeSpan.FromMinutes(10));

            return true;
        }

        /// <summary>
        /// دریافت تمام داده‌های یک دستگاه
        /// </summary>
        public async Task<IEnumerable<DeviceData>> GetDeviceDataAsync(string deviceId)
            => await _deviceDataRepository.GetByDeviceIdAsync(deviceId);

        /// <summary>
        /// دریافت آخرین داده ثبت شده از یک دستگاه
        /// </summary>
        public async Task<DeviceData> GetLatestDeviceDataAsync(string deviceId)
            => await _deviceDataRepository.GetLatestByDeviceIdAsync(deviceId);

        /// <summary>
        /// دریافت داده‌ها بر اساس بازه زمانی
        /// </summary>
        public async Task<IEnumerable<DeviceData>> GetDeviceDataByTimeRangeAsync(string deviceId, DateTime from, DateTime to)
            => await _deviceDataRepository.GetByTimeRangeAsync(deviceId, from, to);

        /// <summary>
        /// دریافت داده‌های دارای تخلف (Speeding, Overtaking, SafeDistance)
        /// </summary>
        public async Task<IEnumerable<DeviceData>> GetViolationDataAsync(string deviceId, string violationType)
            => await _deviceDataRepository.GetByViolationTypeAsync(deviceId, violationType);

        // --- تنظیمات دستگاه ---

        /// <summary>
        /// دریافت لیست تمام دستگاه‌ها
        /// </summary>
        public async Task<IEnumerable<DeviceDto>> GetAllDevicesAsync()
        {
            var devices = await _deviceRepository.GetAllAsync();
            return devices.Select(MapToDeviceDto);
        }

        /// <summary>
        /// دریافت یک دستگاه بر اساس DeviceId
        /// </summary>
        public async Task<DeviceDto> GetDeviceByIdAsync(string deviceId)
        {
            var device = await _deviceRepository.GetByDeviceIdAsync(deviceId);
            return device == null ? null : MapToDeviceDto(device);
        }

        /// <summary>
        /// به‌روزرسانی تنظیمات یک دستگاه
        /// </summary>
        public async Task<bool> UpdateDeviceAsync(string deviceId, DeviceSettingsDto updateDto)
        {
            var device = await _deviceRepository.GetByDeviceIdAsync(deviceId);
            if (device == null)
                return false;

            // --- بروزرسانی تنظیمات عمومی ---
            device.Name = updateDto?.Name ?? device.Name;
            device.Location = updateDto?.Location ?? device.Location;
         //   device.Status = updateDto?.Status ?? device.Status;

            // --- لوپ ---
            device.LoopLengths = updateDto?.LoopLengths ?? device.LoopLengths;
            device.LoopDistances = updateDto?.LoopDistances ?? device.LoopDistances;
            device.LoopOffsets = updateDto?.LoopOffsets ?? device.LoopOffsets;
            device.DistanceOffsets = updateDto?.DistanceOffsets ?? device.DistanceOffsets;

            // --- محور ---
            device.GoRoadId = updateDto?.GoRoadId ?? device.GoRoadId;
            device.BackRoadId = updateDto?.BackRoadId ?? device.BackRoadId;
            device.GoLaneCount = updateDto?.GoLaneCount ?? device.GoLaneCount;
            device.BackLaneCount = updateDto?.BackLaneCount ?? device.BackLaneCount;

            // --- طول خودروها ---
            device.Class1Length = updateDto?.Class1Length ?? device.Class1Length;
            device.Class2Length = updateDto?.Class2Length ?? device.Class2Length;
            device.Class3Length = updateDto?.Class3Length ?? device.Class3Length;
            device.Class4Length = updateDto?.Class4Length ?? device.Class4Length;

            // --- سرعت مجاز ---
            device.LightVehicleSpeedDay = updateDto?.LightVehicleSpeedDay ?? device.LightVehicleSpeedDay;
            device.LightVehicleSpeedNight = updateDto?.LightVehicleSpeedNight ?? device.LightVehicleSpeedNight;
            device.HeavyVehicleSpeedDay = updateDto?.HeavyVehicleSpeedDay ?? device.HeavyVehicleSpeedDay;
            device.HeavyVehicleSpeedNight = updateDto?.HeavyVehicleSpeedNight ?? device.HeavyVehicleSpeedNight;

            // --- ساعت روز/شب ---
            device.DayStartTime = updateDto?.DayStartTime ?? device.DayStartTime;
            device.DayEndTime = updateDto?.DayEndTime ?? device.DayEndTime;

            // --- تنظیمات شبکه ---
            device.Ip1 = updateDto?.Ip1 ?? device.Ip1;
            device.Ip2 = updateDto?.Ip2 ?? device.Ip2;
            device.Ip3 = updateDto?.Ip3 ?? device.Ip3;
            device.Port = updateDto?.Port ?? device.Port;
            device.SummaryTime = updateDto?.SummaryTime ?? device.SummaryTime;
            device.PhoneNumber1 = updateDto?.PhoneNumber1 ?? device.PhoneNumber1;
            device.PhoneNumber2 = updateDto?.PhoneNumber2 ?? device.PhoneNumber2;

            // --- کارت ---
            device.CardState = updateDto?.CardState ?? device.CardState;
            device.CardDirection = updateDto?.CardDirection ?? device.CardDirection;
            device.CardLoopLengths = updateDto?.CardLoopLengths ?? device.CardLoopLengths;

            // --- ذخیره تغییرات ---
            await _deviceRepository.UpdateAsync(device);
            return true;
        }

        /// <summary>
        /// حذف داده‌های قدیمی‌تر از یک تاریخ مشخص
        /// </summary>
        public async Task<bool> DeleteOldDataAsync(string deviceId, DateTime cutoffDate)
        {
            var oldData = await _deviceDataRepository.GetByTimeRangeAsync(deviceId, DateTime.MinValue, cutoffDate);
            if (oldData == null || !oldData.Any())
                return false;

            foreach (var item in oldData)
                await _deviceDataRepository.DeleteAsync(item.Id);

            return true;
        }

        // --- Mapping Methods ---

        /// <summary>
        /// مپ کردن Device به DeviceDto
        /// </summary>
        private DeviceDto MapToDeviceDto(Device device) => new DeviceDto
        {
            DeviceId = device.DeviceId,
            Name = device.Name,
            Location = device.Location,
            Status = device.Status,
            FID = device.FID,
            RID = device.RID
        };

        public Task<DeviceSettingsDto> GetSettingsByDeviceIdAsync(string deviceId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateSettingsAsync(string deviceId, DeviceSettingsDto settings)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateConnectionStatusAsync(string deviceId, bool isConnected)
        {
            throw new NotImplementedException();
        }
    }
}