using System.Threading.Tasks;

namespace DeviceDataProcessor.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context; // کانتکست دیتابیس

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context; // دریافت کانتکست
        }

        public async Task SaveChangesAsync() => await _context.SaveChangesAsync(); // ذخیره تغییرات در دیتابیس
    }
}