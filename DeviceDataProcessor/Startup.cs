using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;
using FluentValidation;
using DeviceDataProcessor.Data; // فضای نام مربوط به داده‌ها
using DeviceDataProcessor.Services; // فضای نام مربوط به خدمات
using DeviceDataProcessor.Validators; // فضای نام مربوط به اعتبارسنجی
using DeviceDataProcessor.Models; // فضای نام مربوط به مدل‌ها
using DeviceDataProcessor.DTOs; // فضای نام مربوط به DTOها
using DeviceDataProcessor.Workers; // فضای نام مربوط به کارگران
using DeviceDataProcessor.Settings; // فضای نام مربوط به تنظیمات
using Microsoft.AspNetCore.Authentication.JwtBearer; // فضای نام مربوط به JWT
using Microsoft.IdentityModel.Tokens; // فضای نام مربوط به توکن
using System.Text;
using Microsoft.OpenApi.Models;
using Serilog; // فضای نام مربوط به رمزنگاری

public class Startup
{
    public IConfiguration Configuration { get; }

    public Startup(IConfiguration configuration)
    {
        Configuration = configuration; // دریافت تنظیمات از فایل پیکربندی
    }

    public void ConfigureServices(IServiceCollection services)
    {

        // ✅ استفاده از Serilog به عنوان لاگر اصلی
        services.AddLogging(loggingBuilder =>
        {
            loggingBuilder.ClearProviders(); // حذف فراهم‌کنندگان پیش‌فرض مثل ConsoleLogger
            loggingBuilder.AddSerilog();     // استفاده از Serilog به عنوان لاگر اصلی
        });


        // اتصال به دیتابیس SQL
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

        // اتصال به Redis
        services.AddSingleton<IConnectionMultiplexer>(ConnectionMultiplexer.Connect(Configuration.GetConnectionString("Redis")));

        // ثبت مخازن (Repositories)
        services.AddScoped<IDeviceRepository, DeviceRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IDeviceDataRepository, DeviceDataRepository>();
        services.AddScoped<IDeviceDataRepository, DeviceDataRepository>();

        // ثبت خدمات (Services)
        services.AddScoped<IDataProcessorService, DataProcessorService>();
        services.AddScoped<IMqttService, MqttService>();

        // پیکربندی JWT
        services.Configure<JwtSettings>(Configuration.GetSection("JwtSettings")); // ثبت تنظیمات JWT
        var jwtSettings = Configuration.GetSection("JwtSettings");
        var key = jwtSettings["Secret"]; // کلید مخفی JWT

        // پیکربندی احراز هویت با JWT
        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key))
            };
        });

        // ثبت خدمات دیگر
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<RedisService>();
        services.AddScoped<ApiService>();
        services.AddScoped<LoggingService>();

        // ثبت سرویس صف پیام
        services.AddScoped<IMessageQueueService>(provider => new MessageQueueService(Configuration["QueueSettings:HostName"]));
        services.AddSingleton<IValidator<LoginRequest>, LoginRequestValidator>(); // ثبت اعتبارسنجی درخواست ورود

        // افزودن HttpClient برای ارسال درخواست‌ها
        services.AddHttpClient();

        // ثبت DataProcessorWorker به عنوان Scoped
        services.AddScoped<DataProcessorWorker>();

        // افزودن کنترلرها
        services.AddControllers();
        services.AddEndpointsApiExplorer(); // افزودن Explorer برای نقاط انتهایی
        services.AddSwaggerGen(c => // افزودن Swagger برای مستندسازی API
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });

            // پیکربندی Swagger برای JWT
            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                In = ParameterLocation.Header,
                Description = "لطفا توکن را وارد کنید",
                Name = "Authorization",
                Type = SecuritySchemeType.ApiKey
            });
            c.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                { new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    }, new string[] {} }
            });
        });
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseSwagger(); // فعال‌سازی Swagger در محیط توسعه
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1"); // آدرس Swagger
                c.RoutePrefix = string.Empty; // برای بارگذاری Swagger UI در ریشه
            });
        }

        app.UseHttpsRedirection(); // فعال‌سازی HTTPS
        app.UseRouting(); // فعال‌سازی Routing
        app.UseAuthentication(); // فعال‌سازی احراز هویت
        app.UseAuthorization(); // فعال‌سازی مجوزها

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers(); // نقشه‌برداری کنترلرها
        });
    }
}