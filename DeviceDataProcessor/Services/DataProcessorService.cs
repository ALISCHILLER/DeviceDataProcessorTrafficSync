using DeviceDataProcessor.Models;
using DeviceDataProcessor.Data;
using DeviceDataProcessor.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DeviceDataProcessor.Services
{
    public class DataProcessorService : IDataProcessorService
    {
        private readonly IRepository<DeviceData> _deviceDataRepository; // مخزن داده‌های دستگاه
        private readonly IMessageQueueService _messageQueueService; // سرویس صف پیام
        private readonly RedisService _redisService; // سرویس Redis

        public DataProcessorService(IRepository<DeviceData> deviceDataRepository, IMessageQueueService messageQueueService, RedisService redisService)
        {
            _deviceDataRepository = deviceDataRepository; // دریافت مخزن داده‌های دستگاه
            _messageQueueService = messageQueueService; // دریافت سرویس صف پیام
            _redisService = redisService; // دریافت سرویس Redis
        }

        public async Task<bool> ProcessDataAsync(DeviceDataDto data)
        {
            await _messageQueueService.EnqueueAsync(data); // افزودن داده به صف
            await _redisService.SetAsync(data.DeviceId, data, TimeSpan.FromMinutes(10)); // کش‌گذاری داده به مدت 10 دقیقه
            return true; // بازگشت موفقیت
        }

        public async Task<IEnumerable<DeviceData>> GetDeviceDataAsync(string deviceId)
        {
            // تلاش برای دریافت داده از Redis
            var cachedData = await _redisService.GetAsync<DeviceDataDto>(deviceId);
            if (cachedData != null)
            {
                return new List<DeviceData> { cachedData }; // بازگشت داده کش‌شده
            }

            // در صورت عدم وجود در Redis، داده‌ها را از دیتابیس دریافت کنید
            return await _deviceDataRepository.GetAllAsync(); // دریافت داده‌های دستگاه
        }
    }
}