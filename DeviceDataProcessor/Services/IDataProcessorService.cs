using DeviceDataProcessor.DTOs;
using DeviceDataProcessor.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DeviceDataProcessor.Services
{
    public interface IDataProcessorService
    {
        Task<bool> ProcessDataAsync(DeviceDataDto data); // پردازش داده
        Task<IEnumerable<DeviceData>> GetDeviceDataAsync(string deviceId); // دریافت داده‌های دستگاه
    }
}