using System.Collections.Generic;
using System.Threading.Tasks;

namespace DeviceDataProcessor.Data
{
    // اینترفیس عمومی برای تمام مخازن داده (Repository)
    public interface IRepository<T> where T : class
    {
     
        Task<IEnumerable<T>> GetAllAsync(); // دریافت تمام موجودیت‌ها
        Task<T> GetByIdAsync(int id); // دریافت موجودیت بر اساس ID
        Task AddAsync(T entity); // افزودن موجودیت جدید
        Task UpdateAsync(T entity); // به‌روزرسانی موجودیت
        Task DeleteAsync(int id); // حذف موجودیت بر اساس ID
    }
}