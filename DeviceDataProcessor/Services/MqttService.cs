using MQTTnet;
using MQTTnet.Server;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using DeviceDataProcessor.DTOs;
using DeviceDataProcessor.Services;

namespace DeviceDataProcessor.Services
{
    // پیاده‌سازی IMqttService برای مدیریت سرور MQTT
    public class MqttService : IMqttService
    {
        private readonly MqttServer _mqttServer; // سرور MQTT
        private readonly IMessageQueueService _messageQueueService; // سرویس صف پیام

        public MqttService(IMessageQueueService messageQueueService)
        {
            var factory = new MqttServerFactory(); // ایجاد کارخانه MQTT
            var mqttServerOptions = new MqttServerOptionsBuilder().WithDefaultEndpoint().Build();
            _mqttServer = factory.CreateMqttServer(mqttServerOptions); // ایجاد سرور MQTT
            _messageQueueService = messageQueueService; // دریافت سرویس صف پیام
        }

        // متد برای شروع سرور MQTT
        public async Task StartAsync(int port)
        {
            var options = new MqttServerOptionsBuilder()
                .WithDefaultEndpoint()
                .WithDefaultEndpointPort(port) // تعیین پورت سرور
                .Build();

            // ثبت رویداد برای دریافت پیام‌ها
            _mqttServer.ApplicationMessageEnqueuedOrDroppedAsync += async args =>
            {
                var message = Encoding.UTF8.GetString(args.ApplicationMessage.Payload); // تبدیل پیام به رشته
                await OnMessageReceived(message); // پردازش پیام
            };

            await _mqttServer.StartAsync(); // شروع به کار سرور
        }

        // متد برای پردازش پیام‌های دریافتی
        private async Task OnMessageReceived(string message)
        {
            try
            {
                var deviceData = JsonSerializer.Deserialize<DeviceDataDto>(message); // سریالیزه کردن پیام به DTO
                if (deviceData != null)
                {
                    await _messageQueueService.EnqueueAsync(deviceData); // ارسال داده به صف پیام
                }
            }
            catch (JsonException ex)
            {
                // ثبت خطا در صورت عدم موفقیت در سریالیزه کردن
                Console.WriteLine($"خطا در سریالیزه کردن پیام: {ex.Message}");
            }
        }

        // متد برای توقف سرور
        public async Task StopAsync() => await _mqttServer.StopAsync(); // توقف سرور
    }
}