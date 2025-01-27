using System.Collections.Generic;
using System.Threading.Tasks;

namespace DeviceDataProcessor.Data
{
    // اینترفیس عمومی برای مخازن
    public interface IRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync(); // دریافت همه موجودیت‌ها
        Task<T> GetByIdAsync(int id); // دریافت موجودیت بر اساس شناسه
        Task AddAsync(T entity); // افزودن موجودیت
        Task UpdateAsync(T entity); // به‌روزرسانی موجودیت
        Task DeleteAsync(int id); // حذف موجودیت
    }
}