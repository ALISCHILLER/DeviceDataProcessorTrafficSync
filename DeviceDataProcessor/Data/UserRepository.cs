using DeviceDataProcessor.Models; // ایمپورت مدل User
using Microsoft.EntityFrameworkCore; // برای کار با Entity Framework Core
using System;
using System.Threading.Tasks;

namespace DeviceDataProcessor.Data
{
    /// <summary>
    /// پیاده‌سازی IUserRepository برای مدیریت داده‌های کاربران
    /// شامل متدهایی برای خواندن، افزودن، ویرایش و حذف کاربران
    /// </summary>
    public class UserRepository : IUserRepository
    {
        // کانتکست دیتابیس برای کار با Entity Framework Core
        private readonly ApplicationDbContext _context;

        /// <summary>
        /// سازنده کلاس که کانتکست دیتابیس را دریافت می‌کند
        /// </summary>
        /// <param name="context">کانتکست EF Core</param>
        public UserRepository(ApplicationDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        /// <summary>
        /// گرفتن کاربر بر اساس نام کاربری (به صورت غیرحساس به حروف)
        /// </summary>
        /// <param name="username">نام کاربری</param>
        /// <returns>کاربر یافت شده یا null</returns>
        public async Task<User> GetByUsernameAsync(string username)
        {
            return await _context.Users
                .FirstOrDefaultAsync(u => u.Username.ToLower() == username.ToLower());
        }

        /// <summary>
        /// گرفتن لیست تمام کاربران
        /// </summary>
        /// <returns>لیست کاربران</returns>
        public async Task<IEnumerable<User>> GetAllAsync()
        {
            return await _context.Users.ToListAsync();
        }

        /// <summary>
        /// گرفتن کاربر بر اساس شناسه منحصر به فرد
        /// </summary>
        /// <param name="id">شناسه کاربر</param>
        /// <returns>کاربر یافت شده یا null</returns>
        public async Task<User> GetByIdAsync(int id)
        {
            return await _context.Users.FindAsync(id);
        }

        /// <summary>
        /// افزودن کاربر جدید به دیتابیس
        /// </summary>
        /// <param name="user">کاربر جدید</param>
        public async Task AddAsync(User user)
        {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync(); // ذخیره تغییرات در دیتابیس
        }

        /// <summary>
        /// به‌روزرسانی اطلاعات کاربر موجود
        /// </summary>
        /// <param name="user">کاربر با اطلاعات جدید</param>
        public async Task UpdateAsync(User user)
        {
            _context.Users.Update(user); // مشخص کردن کاربری که باید به‌روزرسانی شود
            await _context.SaveChangesAsync(); // ذخیره تغییرات در دیتابیس
        }

        /// <summary>
        /// حذف کاربر بر اساس شناسه
        /// </summary>
        /// <param name="id">شناسه کاربر</param>
        public async Task DeleteAsync(int id)
        {
            var user = await _context.Users.FindAsync(id); // یافتن کاربر
            if (user != null) // اگر کاربر وجود داشت
            {
                _context.Users.Remove(user); // آن را برای حذف علامت‌گذاری کن
                await _context.SaveChangesAsync(); // ذخیره تغییرات در دیتابیس
            }
        }
    }
}