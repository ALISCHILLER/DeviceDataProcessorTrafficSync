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
            _dataProcessorService = dataProcessorService;
        }

        [HttpPost("send")]
        public async Task<IActionResult> SendData(DeviceDataDto data)
        {
            var result = await _dataProcessorService.ProcessDataAsync(data);
            if (!result)
                return BadRequest("Failed to process data."); // در صورت ناموفق بودن پردازش، خطا برمی‌گرداند

            return Ok("Data processed successfully."); // در صورت موفقیت، پیام موفقیت برمی‌گرداند
        }

        [HttpGet("device/{deviceId}")]
        public async Task<IActionResult> GetDeviceData(string deviceId)
        {
            var data = await _dataProcessorService.GetDeviceDataAsync(deviceId);
            return Ok(data); // داده‌های مربوط به دستگاه را برمی‌گرداند
        }
    }
}