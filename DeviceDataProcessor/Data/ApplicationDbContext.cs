using Microsoft.EntityFrameworkCore;
using DeviceDataProcessor.Models;

namespace DeviceDataProcessor.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; } // جدول کاربران
        public DbSet<Device> Devices { get; set; } // جدول دستگاه‌ها
        public DbSet<DeviceData> DeviceData { get; set; } // جدول داده‌های دستگاه
        public DbSet<ApiResponse> ApiResponses { get; set; } // جدول پاسخ‌های API
    }
}