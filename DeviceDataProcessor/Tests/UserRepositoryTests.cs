using DeviceDataProcessor.Data;
using DeviceDataProcessor.Models;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace DeviceDataProcessor.Tests
{
    public class UserRepositoryTests
    {
        private readonly ApplicationDbContext _context; // کانتکست دیتابیس
        private readonly UserRepository _userRepository; // مخزن کاربران

        public UserRepositoryTests()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDb") // استفاده از دیتابیس در حافظه
                .Options;

            _context = new ApplicationDbContext(options); // ایجاد کانتکست
            _userRepository = new UserRepository(_context); // ایجاد مخزن کاربران
        }

        [Fact]
        public async Task AddAsync_AddsUserToDatabase()
        {
            var user = new User { Username = "test", PasswordHash = "hash" }; // ایجاد کاربر
            await _userRepository.AddAsync(user); // افزودن کاربر
            await _context.SaveChangesAsync(); // ذخیره تغییرات

            var savedUser = await _context.Users.FirstOrDefaultAsync(u => u.Username == "test"); // پیدا کردن کاربر
            Assert.NotNull(savedUser); // بررسی اینکه کاربر ذخیره شده است
        }
    }
}