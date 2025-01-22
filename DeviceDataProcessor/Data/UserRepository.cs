using DeviceDataProcessor.Models;
using DeviceDataProcessor.Data;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace DeviceDataProcessor.Data
{
    public class UserRepository : IRepository<User>
    {
        private readonly ApplicationDbContext _context; // کانتکست دیتابیس

        public UserRepository(ApplicationDbContext context)
        {
            _context = context; // دریافت کانتکست
        }

        public async Task<IEnumerable<User>> GetAllAsync() => await _context.Users.ToListAsync(); // دریافت همه کاربران
        public async Task<User> GetByIdAsync(int id) => await _context.Users.FindAsync(id); // دریافت کاربر بر اساس شناسه
        public async Task AddAsync(User user) => await _context.Users.AddAsync(user); // افزودن کاربر
        public async Task UpdateAsync(User user) => _context.Users.Update(user); // به‌روزرسانی کاربر
        public async Task DeleteAsync(int id)
        {
            var user = await _context.Users.FindAsync(id); // پیدا کردن کاربر بر اساس شناسه
            if (user != null)
                _context.Users.Remove(user); // حذف کاربر
        }
    }
}