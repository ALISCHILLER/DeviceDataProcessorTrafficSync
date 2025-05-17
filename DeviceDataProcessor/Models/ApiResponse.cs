namespace DeviceDataProcessor.Models
{
    /// <summary>
    /// مدل عمومی برای پاسخ‌دهی از سمت API به کلاینت
    /// </summary>
    public class ApiResponse<T>
    {
        public bool Success { get; set; }             // موفق بودن عملیات
        public string Message { get; set; }           // پیام مرتبط با عملیات
        public T? Data { get; set; }                   // داده برگشتی (می‌تواند null باشد)

        // سازنده‌های کمکی برای استفاده سریع
        public static ApiResponse<T> SuccessResponse(T data, string message = "عملیات با موفقیت انجام شد")
        {
            return new ApiResponse<T> { Success = true, Message = message, Data = data };
        }

        public static ApiResponse<T> FailResponse(string message)
        {
            return new ApiResponse<T> { Success = false, Message = message, Data = default };
        }
    }
}
