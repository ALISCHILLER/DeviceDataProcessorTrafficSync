using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Threading.Tasks;
using DeviceDataProcessor.Workers;
using Serilog;

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = CreateHostBuilder(args).Build();

        // شروع پردازش داده‌ها
        using (var scope = builder.Services.CreateScope())
        {
            var dataProcessorWorker = scope.ServiceProvider.GetRequiredService<DataProcessorWorker>();
            Task.Run(() => dataProcessorWorker.StartProcessingAsync()); // اجرای پردازش داده‌ها در پس‌زمینه
        }


        await builder.RunAsync(); // اجرای برنامه
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseStartup<Startup>(); // استفاده از کلاس Startup
            });
}