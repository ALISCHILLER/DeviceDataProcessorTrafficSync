using Microsoft.AspNetCore.Http;
using System;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;

namespace DeviceDataProcessor.Middleware
{
    /// <summary>
    /// Middleware برای مدیریت خطاها و بازگشت پاسخ استاندارد به کلاینت
    /// </summary>
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next; // دلیگیت درخواست

        public ErrorHandlingMiddleware(RequestDelegate next)
        {
            _next = next ?? throw new ArgumentNullException(nameof(next));
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context); // ادامه زنجیره درخواست
            }
            catch (Exception ex)
            {
                // تنظیمات اولیه پاسخ
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                var response = new
                {
                    StatusCode = context.Response.StatusCode,
                    Message = "An internal server error occurred.",
                    ExceptionMessage = ex.Message,
                    Path = context.Request.Path
                };

                // لاگ خطا در سیستم لاگ‌گیری (اختیاری)
                // Log.Error(ex, "Unhandled exception in pipeline");

                var jsonResponse = JsonSerializer.Serialize(response);
                await context.Response.WriteAsync(jsonResponse);
            }
        }
    }
}