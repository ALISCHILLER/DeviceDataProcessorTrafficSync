using Microsoft.EntityFrameworkCore;
using DeviceDataProcessor.Models;

namespace DeviceDataProcessor.Data
{
    /// <summary>
    /// کانتکست دیتابیس - مدیریت ارتباط با پایگاه داده
    /// </summary>
    public class ApplicationDbContext : DbContext
    {
        /// <summary>
        /// سازنده کانتکست با گزینه‌های EF Core
        /// </summary>
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
            // می‌توانید در اینجا مثلاً Seed Data اولیه را بزنید
        }

        // جداول دیتابیس
        public DbSet<User> Users { get; set; }         // جدول کاربران
        public DbSet<Device> Devices { get; set; }     // جدول دستگاه‌ها
        public DbSet<DeviceData> DeviceData { get; set; } // جدول داده‌های دستگاه
        public DbSet<ApiResponse> ApiResponses { get; set; } // لاگ پاسخ‌های API
    }
}