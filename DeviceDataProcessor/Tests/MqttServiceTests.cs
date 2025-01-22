using DeviceDataProcessor.Services;
using Moq;
using Xunit;
using System.Net.Http;
using System.Threading.Tasks;
using System.Text;
using System.Text.Json;
using DeviceDataProcessor.DTOs;
using Moq.Protected;
using MQTTnet;

namespace DeviceDataProcessor.Tests
{
    public class MqttServiceTests
    {
        private readonly Mock<HttpMessageHandler> _httpMessageHandlerMock; // موک برای تست HTTP
        private readonly HttpClient _httpClient; // کلاینت HTTP
        private readonly MqttService _mqttService; // سرویس MQTT

        public MqttServiceTests()
        {
            _httpMessageHandlerMock = new Mock<HttpMessageHandler>(); // ایجاد موک
            _httpClient = new HttpClient(_httpMessageHandlerMock.Object); // ایجاد کلاینت HTTP
            _mqttService = new MqttService(_httpClient); // ایجاد سرویس MQTT
        }

        [Fact]
        public async Task OnMessageReceived_SendsDataToApi()
        {
            var deviceData = new DeviceDataDto
            {
                DeviceId = "device1",
                Timestamp = DateTime.UtcNow,
                Value = 10.5
            };
            var json = JsonSerializer.Serialize(deviceData); // سریالیزه کردن داده
            var content = new StringContent(json, Encoding.UTF8, "application/json"); // ساخت محتوای JSON

            _httpMessageHandlerMock
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    It.IsAny<HttpRequestMessage>(),
                    It.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage { StatusCode = System.Net.HttpStatusCode.OK }); // پاسخ موفق

            await _mqttService.OnMessageReceived(new MqttApplicationMessageReceivedEventArgs
            {
                ApplicationMessage = new MqttApplicationMessage
                {
                    Payload = Encoding.UTF8.GetBytes(json) // ارسال داده به صورت بایت
                }
            });

            _httpMessageHandlerMock.Protected().Verify(
                "SendAsync",
                Times.Once(),
                It.Is<HttpRequestMessage>(req =>
                    req.Method == HttpMethod.Post
                    && req.RequestUri == new Uri("https://www.example.com/")
                    && req.Content.ReadAsStringAsync().Result == json), // بررسی درخواست
                It.IsAny<CancellationToken>());
        }
    }
}