using DeviceDataProcessor.Data;
using DeviceDataProcessor.Models;

namespace DeviceDataProcessor.Data
{
    /// <summary>
    /// Unit of Work Pattern – مدیریت تمام Repository ها و ذخیره تغییرات در قالب یک واحد
    /// </summary>
    public interface IUnitOfWork
    {
        // --- Repositories ---
        IUserRepository Users { get; }
       /// <summary>
       /// IDeviceRepository Devices { get; }
       /// </summary>
        IDeviceDataRepository DeviceData { get; }

        // --- Save Changes ---
        Task<int> SaveChangesAsync(); // برمی‌گرداند تعداد ردیف‌های تأثیرگذارده
    }
}