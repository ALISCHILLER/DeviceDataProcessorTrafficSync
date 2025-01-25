using MQTTnet;
using MQTTnet.Server;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Text.Json;
using DeviceDataProcessor.DTOs;
using MQTTnet.AspNetCoreEx;
using DeviceDataProcessor.Services; // فرض بر این است که MessageQueueService در این namespace قرار دارد

namespace DeviceDataProcessor.Services
{
    public class MqttService : IMqttService
    {
        private readonly MqttServer _mqttServer; // سرور MQTT
        private readonly HttpClient _httpClient; // کلاینت HTTP
        private readonly IMessageQueueService _messageQueueService; // سرویس صف پیام

        public MqttService(HttpClient httpClient, IMessageQueueService messageQueueService)
        {
            var factory = new MqttServerFactory(); // ایجاد کارخانه MQTT
            var mqttServerOptions = new MqttServerOptionsBuilder().WithDefaultEndpoint().Build();
            _mqttServer = factory.CreateMqttServer(mqttServerOptions); // ایجاد سرور MQTT
            _httpClient = httpClient; // دریافت کلاینت HTTP
            _messageQueueService = messageQueueService; // دریافت سرویس صف پیام
        }

        public async Task StartAsync(int port)
        {
            var options = new MqttServerOptionsBuilder()
                .WithDefaultEndpoint()
                .WithDefaultEndpointPort(port)
                .Build();

            _mqttServer.ApplicationMessageEnqueuedOrDroppedAsync += async args =>
            {
                Console.WriteLine($"Message enqueued or dropped: {args.ApplicationMessage.Topic}");
                var message = Encoding.UTF8.GetString(args.ApplicationMessage.Payload); // تبدیل پیام به رشته
                await OnMessageReceived(message); // پردازش پیام
            };

            await _mqttServer.StartAsync(); // شروع به کار سرور
        }

        private async Task OnMessageReceived(string message)
        {
            var deviceData = JsonSerializer.Deserialize<DeviceDataDto>(message); // سریالیزه کردن پیام به DTO

            if (deviceData != null)
            {
                // ارسال داده به صف پیام
                await _messageQueueService.EnqueueAsync(deviceData);
            }
        }

        public async Task StopAsync() => await _mqttServer.StopAsync(); // توقف سرور
    }
}