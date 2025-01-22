using Microsoft.AspNetCore.Http;
using System;
using System.Net;
using System.Threading.Tasks;

namespace DeviceDataProcessor.Middleware
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next; // دلیگیت درخواست

        public ErrorHandlingMiddleware(RequestDelegate next)
        {
            _next = next; // دریافت دلیگیت
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context); // پردازش درخواست
            }
            catch (Exception ex)
            {
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError; // تنظیم کد خطا
                await context.Response.WriteAsync($"An error occurred: {ex.Message}"); // نوشتن پیام خطا
            }
        }
    }
}