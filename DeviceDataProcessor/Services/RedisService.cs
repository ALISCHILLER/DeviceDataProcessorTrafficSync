using StackExchange.Redis; // استفاده از کتابخانه StackExchange.Redis
using System.Text.Json; // برای سریالیزه کردن و دی‌سریالیزه کردن داده‌ها
using System.Threading.Tasks; // برای استفاده از async و await

namespace DeviceDataProcessor.Services
{
    // سرویس برای مدیریت ارتباط با Redis
    public class RedisService
    {
        private readonly IDatabase _redisDb; // پایگاه داده Redis

        // سازنده که اتصال به Redis را دریافت می‌کند
        public RedisService(IConnectionMultiplexer redis)
        {
            _redisDb = redis.GetDatabase(); // دریافت پایگاه داده
        }

        // متد برای ذخیره داده‌ها در Redis با زمان انقضا
        public async Task SetAsync(string key, object value, TimeSpan expiry)
        {
            var serializedValue = JsonSerializer.Serialize(value); // سریالیزه کردن شیء به JSON
            await _redisDb.StringSetAsync(key, serializedValue, expiry); // ذخیره در Redis با زمان انقضا
        }

        // متد برای دریافت داده‌ها از Redis
        public async Task<T> GetAsync<T>(string key)
        {
            var value = await _redisDb.StringGetAsync(key); // دریافت از Redis
            return value.HasValue ? JsonSerializer.Deserialize<T>(value) : default; // دی‌سریالیزه کردن و بازگشت شیء
        }
    }
}