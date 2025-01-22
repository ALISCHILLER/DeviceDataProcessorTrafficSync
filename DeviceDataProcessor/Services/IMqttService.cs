using System.Threading.Tasks;

namespace DeviceDataProcessor.Services
{
    public interface IMqttService
    {
        Task StartAsync(int port); // شروع سرور MQTT
        Task StopAsync(); // توقف سرور MQTT
    }
}