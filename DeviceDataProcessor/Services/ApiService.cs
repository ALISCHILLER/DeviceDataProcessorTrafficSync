using DeviceDataProcessor.DTOs;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace DeviceDataProcessor.Services
{
    public class ApiService
    {
        private readonly HttpClient _httpClient; // کلاینت HTTP

        public ApiService(HttpClient httpClient)
        {
            _httpClient = httpClient; // دریافت کلاینت HTTP
        }

        public async Task<bool> SendDataAsync(DeviceDataDto data)
        {
            var json = JsonSerializer.Serialize(data); // سریالیزه کردن داده
            var content = new StringContent(json, Encoding.UTF8, "application/json"); // ساخت محتوای JSON

            var response = await _httpClient.PostAsync("https://www.example.com/", content); // ارسال داده به API
            return response.IsSuccessStatusCode; // بررسی موفقیت درخواست
        }
    }
}