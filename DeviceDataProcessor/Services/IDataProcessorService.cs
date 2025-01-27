using DeviceDataProcessor.DTOs;
using DeviceDataProcessor.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DeviceDataProcessor.Services
{
    // اینترفیس برای پردازش داده‌های دریافتی از دستگاه‌ها
    public interface IDataProcessorService
    {
        Task<bool> ProcessDataAsync(DeviceDataDto data); // پردازش داده
        Task<IEnumerable<DeviceData>> GetDeviceDataAsync(string deviceId); // دریافت داده‌های دستگاه
        Task<IEnumerable<Device>> GetAllDevicesAsync(); // دریافت همه دستگاه‌ها
        Task<bool> UpdateDeviceAsync(string deviceId, DeviceUpdateDto updateDto); // به‌روزرسانی دستگاه
    }
}