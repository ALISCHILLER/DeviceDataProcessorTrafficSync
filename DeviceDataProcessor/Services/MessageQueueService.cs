using System.Text.Json;
using RabbitMQ.Client;
using DeviceDataProcessor.DTOs;
using System.Text;
using System.Threading.Channels;

namespace DeviceDataProcessor.Services
{
    public class MessageQueueService : IMessageQueueService
    {
        private readonly ConnectionFactory _factory; // کارخانه RabbitMQ
        private IConnection _connection; // اتصال به RabbitMQ
        private IChannel _channel; // کانال RabbitMQ

        public MessageQueueService(string hostname)
        {
            _factory = new ConnectionFactory() { HostName = hostname }; // ایجاد کارخانه RabbitMQ
        }

        public async Task InitializeAsync()
        {
            _connection = await _factory.CreateConnectionAsync(); // ایجاد اتصال
            _channel = await _connection.CreateChannelAsync(); // ایجاد کانال

            // ایجاد صف
            await _channel.QueueDeclareAsync(
                queue: "device_data_queue",
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null
            );
        }

        public async Task EnqueueAsync(DeviceDataDto data)
        {

            if (_channel == null)
            {
                throw new InvalidOperationException("Channel is not initialized. Call InitializeAsync first.");
            }

            var json = JsonSerializer.Serialize(data); // سریالیزه کردن داده
            var body = Encoding.UTF8.GetBytes(json); // تبدیل به بایت

            var props = new BasicProperties
            {
                Persistent = true          // تعیین اینکه پیام پایدار باشد
            };

            // ارسال پیام به صف
            await Task.Run(() =>
                _channel.BasicPublishAsync(
                    exchange: "",
                    routingKey: "device_data_queue",
                    body: body,
                    basicProperties: props,
                    mandatory: true
                )
            );

            Console.WriteLine($" [x] Sent {json}"); // چاپ پیام ارسال شده
        }

        public async Task<DeviceDataDto> DequeueAsync()
        {
            var result = await Task.Run(() => _channel.BasicGetAsync("device_data_queue", autoAck: false));
            if (result == null)
            {
                return null; // اگر پیام وجود نداشت، null برمی‌گرداند
            }

            var json = Encoding.UTF8.GetString(result.Body.ToArray());
            var deviceData = JsonSerializer.Deserialize<DeviceDataDto>(json); // سریالیزه کردن پیام به DTO

            // تأیید پردازش موفق
            _channel.BasicAckAsync(result.DeliveryTag, false);

            return deviceData; // برگرداندن داده دستگاه
        }

        public void Dispose()
        {
            _channel?.CloseAsync(); // بستن کانال
            _connection?.CloseAsync(); // بستن اتصال
        }
    }
}