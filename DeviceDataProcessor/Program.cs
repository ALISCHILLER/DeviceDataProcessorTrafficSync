using DeviceDataProcessor.Workers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

public class Program
{
    public static void Main(string[] args)
    {
        var host = CreateHostBuilder(args).Build();

        // شروع پردازش داده‌ها
        var dataProcessorWorker = host.Services.GetRequiredService<DataProcessorWorker>();
        Task.Run(() => dataProcessorWorker.StartProcessingAsync()); // اجرای پردازش داده‌ها در پس‌زمینه

        host.Run(); // اجرای برنامه
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseStartup<Startup>(); // استفاده از کلاس Startup
            });
}