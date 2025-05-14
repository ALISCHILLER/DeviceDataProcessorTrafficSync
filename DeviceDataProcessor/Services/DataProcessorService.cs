using DeviceDataProcessor.DTOs;
using DeviceDataProcessor.Models;
using DeviceDataProcessor.Data;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DeviceDataProcessor.Services
{
    // پیاده‌سازی IDataProcessorService برای پردازش داده‌ها
    public class DataProcessorService : IDataProcessorService
    {
        private readonly IDeviceDataRepository _deviceDataRepository; // مخزن داده‌های دستگاه
        private readonly IUserRepository<Device> _deviceRepository; // مخزن دستگاه‌ها
        private readonly IMessageQueueService _messageQueueService; // سرویس صف پیام
        private readonly RedisService _redisService; // سرویس Redis

        public DataProcessorService(IDeviceDataRepository deviceDataRepository, IUserRepository<Device> deviceRepository, IMessageQueueService messageQueueService, RedisService redisService)
        {
            _deviceDataRepository = deviceDataRepository; // دریافت مخزن داده‌های دستگاه
            _deviceRepository = deviceRepository; // دریافت مخزن دستگاه‌ها
            _messageQueueService = messageQueueService; // دریافت سرویس صف پیام
            _redisService = redisService; // دریافت سرویس Redis
        }

        // متد برای پردازش داده‌های دریافتی از دستگاه
        public async Task<bool> ProcessDataAsync(DeviceDataDto data)
        {
            var deviceData = new DeviceData
            {
                DeviceId = data.DeviceId,
                Timestamp = data.Timestamp,
                Value = data.Value
            };
            await _deviceDataRepository.AddAsync(deviceData); // افزودن داده دستگاه به مخزن
            await _messageQueueService.EnqueueAsync(data); // افزودن داده به صف
            await _redisService.SetAsync(data.DeviceId, data, TimeSpan.FromMinutes(10)); // کش‌گذاری داده به مدت 10 دقیقه
            return true; // بازگشت موفقیت
        }

        // متد برای دریافت داده‌های مربوط به یک دستگاه خاص
        public async Task<IEnumerable<DeviceData>> GetDeviceDataAsync(string deviceId)
        {
            return await _deviceDataRepository.GetByDeviceIdAsync(deviceId); // دریافت داده‌های دستگاه بر اساس شناسه
        }

        // متد برای دریافت لیست تمام دستگاه‌ها
        public async Task<IEnumerable<Device>> GetAllDevicesAsync()
        {
            return await _deviceRepository.GetAllAsync(); // دریافت لیست دستگاه‌ها
        }

        // متد برای به‌روزرسانی اطلاعات دستگاه
        public async Task<bool> UpdateDeviceAsync(string deviceId, DeviceUpdateDto updateDto)
        {
            var device = await _deviceRepository.GetByIdAsync(int.Parse(deviceId)); // دریافت دستگاه بر اساس شناسه
            if (device == null) return false; // در صورت عدم وجود دستگاه، بازگشت false

            device.Name = updateDto.Name; // به‌روزرسانی نام دستگاه
            device.Location = updateDto.Location; // به‌روزرسانی موقعیت دستگاه
            device.Status = updateDto.Status; // به‌روزرسانی وضعیت دستگاه

            await _deviceRepository.UpdateAsync(device); // به‌روزرسانی دستگاه در مخزن
            return true; // بازگشت موفقیت
        }
    }
}