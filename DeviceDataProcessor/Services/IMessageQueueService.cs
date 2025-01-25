using System.Threading.Tasks;
using DeviceDataProcessor.DTOs;

namespace DeviceDataProcessor.Services
{
    public interface IMessageQueueService
    {
        Task EnqueueAsync(DeviceDataDto data); // افزودن داده به صف

        Task<DeviceDataDto> DequeueAsync(); // دریافت داده از صف
    }
}