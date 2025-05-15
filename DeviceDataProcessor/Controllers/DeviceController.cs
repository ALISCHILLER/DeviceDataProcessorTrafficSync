using Microsoft.AspNetCore.Mvc;
using DeviceDataProcessor.Services;
using DeviceDataProcessor.DTOs;
using System.Threading.Tasks;
using DeviceDataProcessor.Models;

namespace DeviceDataProcessor.Controllers
{
    /// <summary>
    /// کنترلر مدیریت دستگاه‌ها – شامل متدهای CRUD و تنظیمات گسترده دستگاه
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class DeviceController : ControllerBase
    {
        private readonly IDataProcessorService _deviceService;

        public DeviceController(IDataProcessorService deviceService)
        {
            _deviceService = deviceService;
        }

        // --- عمومی ---

        /// <summary>
        /// دریافت لیست تمام دستگاه‌ها
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetAllDevices()
        {
            var devices = await _deviceService.GetAllDevicesAsync();
            return Ok(devices);
        }

        /// <summary>
        /// دریافت اطلاعات یک دستگاه بر اساس DeviceId
        /// </summary>
        [HttpGet("{deviceId}")]
        public async Task<IActionResult> GetDevice(string deviceId)
        {
            var device = await _deviceService.GetDeviceByIdAsync(deviceId);
            if (device == null)
                return NotFound(new { Message = "دستگاهی با این شناسه یافت نشد." });

            return Ok(device);
        }

        // --- تنظیمات دستگاه ---

        /// <summary>
        /// دریافت تنظیمات کامل یک دستگاه
        /// </summary>
        [HttpGet("{deviceId}/settings")]
        public async Task<IActionResult> GetDeviceSettings(string deviceId)
        {
            var settings = await _deviceService.GetSettingsByDeviceIdAsync(deviceId);
            if (settings == null)
                return NotFound(new { Message = "تنظیمات دستگاه یافت نشد." });

            return Ok(settings);
        }

        /// <summary>
        /// به‌روزرسانی تنظیمات یک دستگاه
        /// </summary>
        [HttpPut("{deviceId}/settings")]
        public async Task<IActionResult> UpdateDeviceSettings(string deviceId, [FromBody] DeviceSettingsDto updateDto)
        {
            if (updateDto == null)
                return BadRequest(new { Message = "داده‌های به‌روزرسانی نمی‌تواند خالی باشد." });

            var result = await _deviceService.UpdateSettingsAsync(deviceId, updateDto);

            if (!result)
                return BadRequest(new { Message = "بروزرسانی دستگاه با شکست مواجه شد." });

            return Ok(new { Message = "تنظیمات دستگاه با موفقیت به‌روز شد." });
        }

        /// <summary>
        /// به‌روزرسانی وضعیت دستگاه (Online / Offline)
        /// </summary>
        [HttpPut("{deviceId}/status")]
        public async Task<IActionResult> UpdateDeviceStatus(string deviceId, [FromBody] string status)
        {
            var isValidStatus = Enum.TryParse<DeviceStatus>(status, true, out var parsedStatus);
            if (!isValidStatus)
                return BadRequest(new { Message = "وضعیت نامعتبر. فقط 'Online' یا 'Offline' قابل قبول است." });

            var device = await _deviceService.GetDeviceByIdAsync(deviceId);
            if (device == null)
                return NotFound(new { Message = "دستگاهی با این شناسه یافت نشد." });

            // فرض: داریم از UpdateDeviceAsync برای بروزرسانی وضعیت دستگاه استفاده می‌کنیم
            var updateDto = new DeviceUpdateDto
            {
                Status = parsedStatus
            };

            var result = await _deviceService.UpdateDeviceAsync(deviceId, updateDto);

            if (!result)
                return BadRequest(new { Message = "به‌روزرسانی وضعیت دستگاه با شکست مواجه شد." });

            return Ok(new { Message = "وضعیت دستگاه با موفقیت به‌روز شد." });
        }

        // --- داده‌های دستگاه ---

        /// <summary>
        /// دریافت تمام داده‌های یک دستگاه در یک بازه زمانی
        /// </summary>
        [HttpGet("{deviceId}/data")]
        public async Task<IActionResult> GetDeviceData(
            string deviceId,
            [FromQuery] DateTime from,
            [FromQuery] DateTime to)
        {
            var data = await _deviceService.GetDeviceDataByTimeRangeAsync(deviceId, from, to);

            if (data == null || !data.Any())
                return NotFound(new { Message = "داده‌ای در بازه زمانی مشخص یافت نشد." });

            return Ok(data);
        }

        /// <summary>
        /// دریافت آخرین داده ثبت شده از یک دستگاه
        /// </summary>
        [HttpGet("{deviceId}/data/latest")]
        public async Task<IActionResult> GetLatestDeviceData(string deviceId)
        {
            var data = await _deviceService.GetLatestDeviceDataAsync(deviceId);
            if (data == null)
                return NotFound(new { Message = "داده‌ای برای این دستگاه یافت نشد." });

            return Ok(data);
        }

        /// <summary>
        /// دریافت داده‌های دارای تخلف
        /// </summary>
        [HttpGet("{deviceId}/data/violations")]
        public async Task<IActionResult> GetViolationData(string deviceId, [FromQuery] string violationType)
        {
            var data = await _deviceService.GetViolationDataAsync(deviceId, violationType);

            if (data == null || !data.Any())
                return NotFound(new { Message = "داده‌ای با این نوع تخلف یافت نشد." });

            return Ok(data);
        }

        /// <summary>
        /// حذف داده‌های قدیمی‌تر از یک تاریخ مشخص
        /// </summary>
        [HttpDelete("{deviceId}/data/clear")]
        public async Task<IActionResult> DeleteOldData(string deviceId, [FromQuery] DateTime cutoffDate)
        {
            var result = await _deviceService.DeleteOldDataAsync(deviceId, cutoffDate);

            if (!result)
                return BadRequest(new { Message = "حذف داده‌ها با شکست مواجه شد." });

            return Ok(new { Message = "داده‌های قدیمی با موفقیت حذف شدند." });
        }
    }
}