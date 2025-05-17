using MQTTnet;
using MQTTnet.Server;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using DeviceDataProcessor.DTOs;
using DeviceDataProcessor.Services;
using Serilog;

namespace DeviceDataProcessor.Services
{
    // پیاده‌سازی IMqttService برای مدیریت سرور MQTT
    public class MqttService : IMqttService
    {
        private readonly MqttServer _mqttServer; // سرور MQTT
        private readonly IMessageQueueService _messageQueueService; // سرویس صف پیام
        private readonly ILogger<MqttService> _logger;
        public MqttService(IMessageQueueService messageQueueService, ILogger<MqttService> logger)
        {
            var factory = new MqttServerFactory(); // ایجاد کارخانه MQTT
            var mqttServerOptions = new MqttServerOptionsBuilder().WithDefaultEndpoint().Build();
            _mqttServer = factory.CreateMqttServer(mqttServerOptions); // ایجاد سرور MQTT
            _messageQueueService = messageQueueService; // دریافت سرویس صف پیام
            _logger = logger; // ✅ استفاده از Serilog با Context

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
                try
                {

                
                    var message = Encoding.UTF8.GetString(args.ApplicationMessage.Payload); // تبدیل پیام به رشته
                    _logger.LogDebug("پیام MQTT دریافت شد: {Message}", message);
                    await OnMessageReceived(message); // پردازش پیام
                }
                catch (Exception ex)
                {
                    // ✅ لاگ خطا در مرحله دریافت پیام
                    _logger.LogError(ex, "خطای عمومی در دریافت پیام از MQTT");
                }
            };

            await _mqttServer.StartAsync(); // شروع به کار سرور
        }

        // متد برای پردازش پیام‌های دریافتی
        private async Task OnMessageReceived(string message)
        {
            try
            {
                // سریالیزاسیون JSON به DTO دستگاه
                var deviceData = JsonSerializer.Deserialize<DeviceDataDto>(message);

                if (deviceData == null)
                {
                    _logger.LogWarning("داده دریافتی از MQTT نامعتبر است.");
                    return;
                }

                _logger.LogInformation("داده دریافت شده از دستگاه {DeviceId}", deviceData.DeviceId);

                // ارسال داده به صف پیام برای پردازش بعدی
                await _messageQueueService.EnqueueAsync(deviceData);
            }
            catch (JsonException ex)
            {
                // ✅ لاگ خطا در سریالیزه کردن JSON
                _logger.LogError(ex, "خطا در سریالیزه کردن داده از MQTT");
            }
            catch (Exception ex)
            {
                // ✅ مدیریت سایر خطاهای غیرمنتظره
                _logger.LogError(ex, "خطای غیرمنتظره در پردازش داده MQTT");
            }
        }

        // متد برای توقف سرور
        public async Task StopAsync() => await _mqttServer.StopAsync(); // توقف سرور
    }
}