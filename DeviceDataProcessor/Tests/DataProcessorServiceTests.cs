using DeviceDataProcessor.Services;
using DeviceDataProcessor.Data;
using DeviceDataProcessor.Models;
using Microsoft.EntityFrameworkCore;
using Xunit;
using DeviceDataProcessor.DTOs;
using Moq; // برای استفاده از موک
using StackExchange.Redis; // برای استفاده از Redis

namespace DeviceDataProcessor.Tests
{
    public class DataProcessorServiceTests
    {
        private readonly ApplicationDbContext _context; // کانتکست دیتابیس
        private readonly DataProcessorService _dataProcessorService; // سرویس پردازش داده

        public DataProcessorServiceTests()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDb") // استفاده از دیتابیس در حافظه
                .Options;

            _context = new ApplicationDbContext(options); // ایجاد کانتکست

            // ایجاد موک برای IMessageQueueService
            var messageQueueServiceMock = new Mock<IMessageQueueService>();
            messageQueueServiceMock.Setup(m => m.EnqueueAsync(It.IsAny<DeviceDataDto>())).Returns(Task.CompletedTask); // تنظیم موک

            // ایجاد موک برای IConnectionMultiplexer
            var redisMock = new Mock<IConnectionMultiplexer>();
            var redisDbMock = new Mock<IDatabase>();
            redisMock.Setup(m => m.GetDatabase(It.IsAny<int>(), It.IsAny<object>())).Returns(redisDbMock.Object);

            // ایجاد سرویس Redis
            var redisService = new RedisService(redisMock.Object); // استفاده از موک Redis

            // ایجاد سرویس پردازش داده
            _dataProcessorService = new DataProcessorService(
                new DeviceDataRepository(_context), // استفاده از DeviceDataRepository
                new DeviceRepository(_context), // استفاده از DeviceRepository
                messageQueueServiceMock.Object, // استفاده از موک
                redisService // ارسال RedisService
            );
        }

        [Fact]
        public async Task ProcessDataAsync_ValidData_ReturnsTrue()
        {
            var data = new DeviceDataDto { DeviceId = "device1", Timestamp = DateTime.UtcNow, Value = 10.5 }; // ایجاد داده دستگاه
            var result = await _dataProcessorService.ProcessDataAsync(data); // پردازش داده
            Assert.True(result); // بررسی اینکه نتیجه درست است
        }
    }
}