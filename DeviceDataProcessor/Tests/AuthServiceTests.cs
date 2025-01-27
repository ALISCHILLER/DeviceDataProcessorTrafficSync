using DeviceDataProcessor.Services;
using DeviceDataProcessor.Data;
using DeviceDataProcessor.Models;
using Microsoft.EntityFrameworkCore;
using Xunit;
using DeviceDataProcessor.Settings;
using Microsoft.Extensions.Options;

namespace DeviceDataProcessor.Tests
{
    public class AuthServiceTests
    {
        private readonly ApplicationDbContext _context; // کانتکست دیتابیس
        private readonly AuthService _authService; // سرویس احراز هویت

        public AuthServiceTests()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDb") // استفاده از دیتابیس در حافظه
                .Options;

            _context = new ApplicationDbContext(options); // ایجاد کانتکست

            // تنظیمات JWT
            var jwtSettings = Options.Create(new JwtSettings
            {
                Secret = "YourSuperSecretKeyHere",
                ExpiryInHours = 1
            });

            _authService = new AuthService(new UserRepository(_context), jwtSettings); // ایجاد سرویس احراز هویت
        }

        [Fact]
        public async Task AuthenticateAsync_ValidCredentials_ReturnsToken()
        {
            var user = new User { Username = "test", PasswordHash = "hash" }; // ایجاد کاربر
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            var token = await _authService.AuthenticateAsync("test", "hash"); // احراز هویت کاربر
            Assert.NotNull(token); // بررسی اینکه توکن بازگشت داده شده است
        }

        [Fact]
        public async Task AuthenticateAsync_InvalidCredentials_ReturnsNull()
        {
            var token = await _authService.AuthenticateAsync("invalidUser", "invalidPassword"); // احراز هویت کاربر نامعتبر
            Assert.Null(token); // بررسی اینکه توکن null برمی‌گرداند
        }
    }
}