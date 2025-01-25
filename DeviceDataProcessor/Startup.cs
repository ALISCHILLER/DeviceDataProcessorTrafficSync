using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using DeviceDataProcessor.Data;
using DeviceDataProcessor.Services;
using DeviceDataProcessor.Validators;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;
using Microsoft.Extensions.Configuration;
using DeviceDataProcessor.Models;
using DeviceDataProcessor.DTOs;
using DeviceDataProcessor.Workers; // اضافه کردن namespace مربوط به Worker

public class Startup
{
    public IConfiguration Configuration { get; }

    public Startup(IConfiguration configuration)
    {
        Configuration = configuration; // دریافت تنظیمات
    }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"))); // اتصال به دیتابیس SQL

        services.AddSingleton<IConnectionMultiplexer>(ConnectionMultiplexer.Connect(Configuration.GetConnectionString("Redis"))); // اتصال به Redis
        services.AddScoped<IRepository<Device>, DeviceRepository>(); // افزودن مخزن دستگاه‌ها
        services.AddScoped<IRepository<User>, UserRepository>(); // افزودن مخزن کاربران
        services.AddScoped<IDataProcessorService, DataProcessorService>(); // افزودن سرویس پردازش داده
        services.AddScoped<IMqttService, MqttService>(); // افزودن سرویس MQTT
        services.AddScoped<IAuthService, AuthService>(); // افزودن سرویس احراز هویت
        services.AddScoped<RedisService>(); // افزودن سرویس Redis
        services.AddScoped<ApiService>(); // افزودن سرویس API
        services.AddScoped<LoggingService>(); // افزودن سرویس لاگینگ
        services.AddScoped<IMessageQueueService>(provider => new MessageQueueService(Configuration["QueueSettings:HostName"])); // افزودن سرویس صف پیام
        services.AddSingleton<IValidator<LoginRequest>, LoginRequestValidator>(); // افزودن اعتبارسنجی درخواست ورود

        services.AddHttpClient(); // افزودن HttpClient برای ارسال درخواست‌ها
        services.AddSingleton<DataProcessorWorker>(); // ثبت DataProcessorWorker به عنوان سرویس

        services.AddControllers(); // افزودن کنترلرها
        services.AddEndpointsApiExplorer(); // افزودن Explorer برای نقاط انتهایی
        services.AddSwaggerGen(); // افزودن Swagger برای مستندسازی API
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseSwagger(); // فعال‌سازی Swagger در محیط توسعه
            app.UseSwaggerUI(); // فعال‌سازی UI Swagger
        }

        app.UseHttpsRedirection(); // فعال‌سازی HTTPS
        app.UseRouting(); // فعال‌سازی Routing
        app.UseAuthorization(); // فعال‌سازی مجوزها
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers(); // نقشه‌برداری کنترلرها
        });
    }
}