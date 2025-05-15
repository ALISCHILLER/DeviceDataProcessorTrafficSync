using Microsoft.AspNetCore.Mvc;
using DeviceDataProcessor.Services;
using DeviceDataProcessor.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DeviceDataProcessor.Controllers
{
    /// <summary>
    /// کنترلر مدیریت داده‌های دستگاه – شامل متدهای ارسال، دریافت و به‌روزرسانی داده
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class DataController : ControllerBase
    {
        private readonly IDataProcessorService _dataProcessorService;

        public DataController(IDataProcessorService dataProcessorService)
        {
            _dataProcessorService = dataProcessorService;
        }

        // --- داده‌های دستگاه ---

        /// <summary>
        /// ارسال داده جدید از دستگاه به سرور
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /api/data/send
        ///     {
        ///       "deviceId": "D123",
        ///       "timestamp": "2025-04-05T10:00:00Z",
        ///       "st": "10:00:00",
        ///       "et": "10:10:00",
        ///       "c1": 15,
        ///       "c2": 3,
        ///       "c3": 2,
        ///       "c4": 0,
        ///       "c5": 0,
        ///       "asp": 60.5,
        ///       "so": 2,
        ///       "oo": 1,
        ///       "esd": 0
        ///     }
        ///
        /// </remarks>
        [HttpPost("send")]
        public async Task<IActionResult> SendData([FromBody] DeviceDataDto data)
        {
            if (data == null)
                return BadRequest("داده‌ی دستگاه نمی‌تواند خالی باشد.");

            var result = await _dataProcessorService.ProcessDataAsync(data);
            if (!result)
                return StatusCode(StatusCodes.Status500InternalServerError, new { Message = "خطا در پردازش داده" });

            return Ok(new { Message = "داده با موفقیت ذخیره شد.", Data = data });
        }

        /// <summary>
        /// دریافت تمام داده‌های یک دستگاه بر اساس DeviceId
        /// </summary>
        [HttpGet("device/{deviceId}")]
        public async Task<IActionResult> GetDeviceData(string deviceId)
        {
            var data = await _dataProcessorService.GetDeviceDataAsync(deviceId);

            if (data == null || !data.Any())
                return NotFound("هیچ داده‌ای برای این دستگاه یافت نشد.");

            return Ok(data);
        }

        /// <summary>
        /// دریافت آخرین داده ثبت شده از یک دستگاه
        /// </summary>
        [HttpGet("device/{deviceId}/latest")]
        public async Task<IActionResult> GetLatestDeviceData(string deviceId)
        {
            var data = await _dataProcessorService.GetLatestDeviceDataAsync(deviceId);

            if (data == null)
                return NotFound("داده‌ای یافت نشد.");

            return Ok(data);
        }

        /// <summary>
        /// دریافت داده‌ها بر اساس بازه زمانی
        /// </summary>
        [HttpGet("device/{deviceId}/range")]
        public async Task<IActionResult> GetDataByTimeRange(
            string deviceId,
            [FromQuery] DateTime from,
            [FromQuery] DateTime to)
        {
            var data = await _dataProcessorService.GetDataByTimeRangeAsync(deviceId, from, to);

            if (data == null || !data.Any())
                return NotFound("داده‌ای در بازه زمانی مشخص یافت نشد.");

            return Ok(data);
        }

        /// <summary>
        /// دریافت داده‌های دارای تخلف
        /// </summary>
        [HttpGet("device/{deviceId}/violations")]
        public async Task<IActionResult> GetViolationData(
            string deviceId,
            [FromQuery] string violationType)
        {
            var data = await _dataProcessorService.GetViolationDataAsync(deviceId, violationType);

            if (data == null || !data.Any())
                return NotFound("داده‌ای با این نوع تخلف یافت نشد.");

            return Ok(data);
        }

        // --- دستگاه‌ها ---

        /// <summary>
        /// دریافت لیست تمام دستگاه‌ها
        /// </summary>
        [HttpGet("devices")]
        public async Task<IActionResult> GetAllDevices()
        {
            var devices = await _dataProcessorService.GetAllDevicesAsync();
            return Ok(devices);
        }

        /// <summary>
        /// دریافت اطلاعات یک دستگاه بر اساس DeviceId
        /// </summary>
        [HttpGet("device/settings/{deviceId}")]
        public async Task<IActionResult> GetDeviceSettings(string deviceId)
        {
            var settings = await _dataProcessorService.GetDeviceByIdAsync(deviceId);
            if (settings == null)
                return NotFound("دستگاهی با این شناسه یافت نشد.");

            return Ok(settings);
        }

        /// <summary>
        /// به‌روزرسانی تنظیمات یک دستگاه
        /// </summary>
        [HttpPut("device/settings/{deviceId}")]
        public async Task<IActionResult> UpdateDeviceSettings(string deviceId, [FromBody] DeviceSettingsDto updateDto)
        {
            if (string.IsNullOrEmpty(deviceId))
                return BadRequest("شناسه دستگاه الزامی است.");

            if (updateDto == null)
                return BadRequest("اطلاعات به‌روزرسانی نمی‌تواند خالی باشد.");

            var result = await _dataProcessorService.UpdateSettingsAsync(deviceId, updateDto);

            if (!result)
                return BadRequest("بروزرسانی دستگاه با شکست مواجه شد.");

            return Ok("تنظیمات دستگاه با موفقیت به‌روز شد.");
        }

        /// <summary>
        /// حذف داده‌های قدیمی‌تر از یک تاریخ مشخص
        /// </summary>
        [HttpDelete("device/{deviceId}/clear-data")]
        public async Task<IActionResult> DeleteOldData(string deviceId, [FromQuery] DateTime cutoffDate)
        {
            var result = await _dataProcessorService.DeleteOldDataAsync(deviceId, cutoffDate);

            if (!result)
                return BadRequest("حذف داده‌ها با شکست مواجه شد.");

            return Ok("داده‌های قدیمی با موفقیت حذف شدند.");
        }
    }
}