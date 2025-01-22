using System.Threading.Tasks;

namespace DeviceDataProcessor.Data
{
    public interface IUnitOfWork
    {
        Task SaveChangesAsync(); // متد برای ذخیره تغییرات در دیتابیس
    }
}