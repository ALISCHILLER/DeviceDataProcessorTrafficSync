using DeviceDataProcessor.DTOs;
using DeviceDataProcessor.Services;
using System.Text;
using System.Text.Json;

namespace DeviceDataProcessor.Workers
{
    public class DataProcessorWorker
    {
        private readonly IMessageQueueService _messageQueueService;
        private readonly HttpClient _httpClient;

        public DataProcessorWorker(IMessageQueueService messageQueueService, HttpClient httpClient)
        {
            _messageQueueService = messageQueueService;
            _httpClient = httpClient;
        }

        public async Task StartProcessingAsync()
        {
            while (true)
            {
                // دریافت پیام از صف
                var result = await _messageQueueService.DequeueAsync();
                if (result != null)
                {
                    var response = await SendDataToApiAsync(result);
                    if (!response.IsSuccessStatusCode)
                    {
                        // مدیریت خطا
                    }
                }
                await Task.Delay(100); // تاخیر برای جلوگیری از بار زیاد
            }
        }

        private async Task<HttpResponseMessage> SendDataToApiAsync(DeviceDataDto data)
        {
            var json = JsonSerializer.Serialize(data);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            return await _httpClient.PostAsync("https://www.example.com/", content);
        }
    }
}
