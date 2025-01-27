using DeviceDataProcessor.Services;
using DeviceDataProcessor.DTOs;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace DeviceDataProcessor.Workers
{
    // Worker برای پردازش داده‌ها در پس‌زمینه
    public class DataProcessorWorker
    {
        private readonly IMessageQueueService _messageQueueService; // سرویس صف پیام
        private readonly HttpClient _httpClient; // کلاینت HTTP

        public DataProcessorWorker(IMessageQueueService messageQueueService, HttpClient httpClient)
        {
            _messageQueueService = messageQueueService; // دریافت سرویس صف پیام
            _httpClient = httpClient; // دریافت کلاینت HTTP
        }

        // متد برای شروع پردازش داده‌ها
        public async Task StartProcessingAsync()
        {
            // اطمینان از اینکه سرویس صف پیام مقداردهی شده است
            await _messageQueueService.InitializeAsync(); // فراخوانی متد InitializeAsync

            while (true)
            {
                var result = await _messageQueueService.DequeueAsync(); // دریافت پیام از صف
                if (result != null)
                {
                    var response = await SendDataToApiAsync(result); // ارسال داده به API
                    if (!response.IsSuccessStatusCode)
                    {
                        // مدیریت خطا
                    }
                }
                await Task.Delay(100); // تاخیر برای جلوگیری از بار زیاد
            }
        }

        // متد برای ارسال داده به API
        private async Task<HttpResponseMessage> SendDataToApiAsync(DeviceDataDto data)
        {
            var json = JsonSerializer.Serialize(data); // سریالیزه کردن داده
            var content = new StringContent(json, Encoding.UTF8, "application/json"); // ساخت محتوای JSON
            return await _httpClient.PostAsync("https://www.example.com/", content); // ارسال داده به API
        }
    }
}