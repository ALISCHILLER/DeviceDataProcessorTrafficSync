using DeviceDataProcessor.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DeviceDataProcessor.Data
{
    // اینترفیس برای مدیریت داده‌های دستگاه
    public interface IDeviceDataRepository : IRepository<DeviceData>
    {
        Task<IEnumerable<DeviceData>> GetByDeviceIdAsync(string deviceId); // متد برای دریافت داده‌ها بر اساس شناسه دستگاه
    }
}