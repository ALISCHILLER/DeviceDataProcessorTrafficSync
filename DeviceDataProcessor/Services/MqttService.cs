using MQTTnet;
using MQTTnet.Server;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Text.Json;
using DeviceDataProcessor.DTOs;
using MQTTnet.AspNetCoreEx;

namespace DeviceDataProcessor.Services
{
    public class MqttService : IMqttService
    {
        private readonly MqttServer _mqttServer; // سرور MQTT
        private readonly HttpClient _httpClient; // کلاینت HTTP

        public MqttService(HttpClient httpClient)
        {
            var factory = new MqttServerFactory(); // ایجاد کارخانه MQTT
            var mqttServerOptions = new MqttServerOptionsBuilder().WithDefaultEndpoint().Build();
            _mqttServer = factory.CreateMqttServer(mqttServerOptions); // ایجاد سرور MQTT
            _httpClient = httpClient; // دریافت کلاینت HTTP
        }

        public async Task StartAsync(int port)
        {
            var options = new MqttServerOptionsBuilder()
                .WithDefaultEndpoint()
                .WithDefaultEndpointPort(port)
            .Build();

         

            _mqttServer.InterceptingPublishAsync += async args =>
            {
                Console.WriteLine($"Message published: {args.ApplicationMessage.Topic}");
                // Custom handling for the message
                await Task.CompletedTask;
            };


          //  _mqttServer.ApplicationMessageEnqueuedOrDroppedAsync += OnMessageReceived; // تنظیم رویداد دریافت پیام

            _mqttServer.ApplicationMessageEnqueuedOrDroppedAsync += async args =>// تنظیم رویداد دریافت پیام
            {
                Console.WriteLine($"Message enqueued or dropped: {args.ApplicationMessage.Topic}");
                var message = Encoding.UTF8.GetString(args.ApplicationMessage.Payload); // تبدیل پیام به رشته
                OnMessageReceived(message);
                // Handle the message here
                await Task.CompletedTask;
            };

            await _mqttServer.StartAsync(); // شروع به کار سرور
        }

        private async Task OnMessageReceived(String message)
        {
         //   var message = Encoding.UTF8.GetString(e.ApplicationMessage.Payload); // تبدیل پیام به رشته
            var deviceData = JsonSerializer.Deserialize<DeviceDataDto>(message); // سریالیزه کردن پیام به DTO

            if (deviceData != null)
            {
                await SendDataToApiAsync(deviceData); // ارسال داده به API خارجی
            }
        }

        private async Task SendDataToApiAsync(DeviceDataDto data)
        {
            var json = JsonSerializer.Serialize(data); // سریالیزه کردن داده
            var content = new StringContent(json, Encoding.UTF8, "application/json"); // ساخت محتوای JSON

            var response = await _httpClient.PostAsync("https://www.example.com/", content); // ارسال داده به API
            if (!response.IsSuccessStatusCode)
            {
                // ثبت خطا در صورت عدم موفقیت
            }
        }

        public async Task StopAsync() => await _mqttServer.StopAsync(); // توقف سرور
    }
}