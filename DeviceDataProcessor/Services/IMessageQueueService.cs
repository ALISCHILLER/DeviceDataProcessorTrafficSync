using System.Threading.Tasks;
using DeviceDataProcessor.DTOs;

namespace DeviceDataProcessor.Services
{
    public interface IMessageQueueService
    {
        Task EnqueueAsync(DeviceDataDto data); // افزودن داده به صف
    }
}