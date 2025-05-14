using DeviceDataProcessor.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DeviceDataProcessor.Data
{
    /// <summary>
    /// پیاده‌سازی IRepository<User> برای مدیریت کاربران
    /// </summary>
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context; // کانتکست Entity Framework Core

        public UserRepository(ApplicationDbContext context)
        {
            _context = context; // دریافت کانتکست از طریق Dependency Injection
        }

        /// <summary>
        /// دریافت کاربر بر اساس نام کاربری
        /// </summary>
        public async Task<User> GetByUsernameAsync(string username)
            => await _context.Users.FirstOrDefaultAsync(u => u.Username == username);

        /// <summary>
        /// دریافت تمام کاربران
        /// </summary>
        public async Task<IEnumerable<User>> GetAllAsync()
            => await _context.Users.ToListAsync();

        /// <summary>
        /// دریافت کاربر بر اساس شناسه
        /// </summary>
        public async Task<User> GetByIdAsync(int id)
            => await _context.Users.FindAsync(id);

        /// <summary>
        /// افزودن کاربر جدید به دیتابیس
        /// </summary>
        public async Task AddAsync(User user)
        {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// به‌روزرسانی کاربر
        /// </summary>
        public async Task UpdateAsync(User user)
            => _context.Users.Update(user);

        /// <summary>
        /// حذف کاربر بر اساس شناسه
        /// </summary>
        public async Task DeleteAsync(int id)
        {
            var user = await _context.Users.FindAsync(id); // پیدا کردن کاربر
            if (user != null)
                _context.Users.Remove(user); // اگر کاربر وجود داشت، حذف کن
        }
    }
}