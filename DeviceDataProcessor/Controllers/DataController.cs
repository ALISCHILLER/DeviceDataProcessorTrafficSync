using Microsoft.AspNetCore.Mvc;
using DeviceDataProcessor.Services;
using DeviceDataProcessor.DTOs;

namespace DeviceDataProcessor.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DataController : ControllerBase
    {
        private readonly IDataProcessorService _dataProcessorService; // سرویس پردازش داده‌ها

        public DataController(IDataProcessorService dataProcessorService)
        {
            _dataProcessorService = dataProcessorService; // دریافت سرویس پردازش داده‌ها
        }

        // متد برای ارسال داده‌های دستگاه
        [HttpPost("send")]
        public async Task<IActionResult> SendData(DeviceDataDto data)
        {
            var result = await _dataProcessorService.ProcessDataAsync(data); // پردازش داده‌ها
            if (!result)
                return BadRequest("Failed to process data."); // در صورت ناموفق بودن پردازش، خطا برمی‌گرداند

            return Ok("Data processed successfully."); // در صورت موفقیت، پیام موفقیت برمی‌گرداند
        }

        // متد برای دریافت داده‌های مربوط به یک دستگاه خاص
        [HttpGet("device/{deviceId}")]
        public async Task<IActionResult> GetDeviceData(string deviceId)
        {
            var data = await _dataProcessorService.GetDeviceDataAsync(deviceId); // دریافت داده‌ها
            return Ok(data); // داده‌های مربوط به دستگاه را برمی‌گرداند
        }

        // متد برای دریافت لیست تمام دستگاه‌ها
        [HttpGet("devices")]
        public async Task<IActionResult> GetAllDevices()
        {
            var devices = await _dataProcessorService.GetAllDevicesAsync(); // دریافت لیست دستگاه‌ها
            return Ok(devices); // برگرداندن لیست دستگاه‌ها
        }

        // متد برای به‌روزرسانی اطلاعات دستگاه
        [HttpPut("device/{deviceId}")]
        public async Task<IActionResult> UpdateDevice(string deviceId, DeviceUpdateDto updateDto)
        {
            var result = await _dataProcessorService.UpdateDeviceAsync(deviceId, updateDto); // به‌روزرسانی دستگاه
            if (!result)
                return BadRequest("Failed to update device."); // در صورت ناموفق بودن، خطا برمی‌گرداند
            return Ok("Device updated successfully."); // در صورت موفقیت، پیام موفقیت برمی‌گرداند
        }
    }
}