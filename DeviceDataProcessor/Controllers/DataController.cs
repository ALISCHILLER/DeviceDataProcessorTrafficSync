using Microsoft.AspNetCore.Mvc;
using DeviceDataProcessor.Services;
using DeviceDataProcessor.DTOs;
using DeviceDataProcessor.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DeviceDataProcessor.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DataController : ControllerBase
    {
        private readonly IDataProcessorService _dataProcessorService;

        public DataController(IDataProcessorService dataProcessorService)
        {
            _dataProcessorService = dataProcessorService;
        }

        [HttpPost("send")]
        public async Task<IActionResult> SendData([FromBody] DeviceDataDto data)
        {
            if (data == null)
                return BadRequest(ApiResponse<string>.FailResponse("داده‌ی دستگاه نمی‌تواند خالی باشد."));

            var result = await _dataProcessorService.ProcessDataAsync(data);
            if (!result)
                return StatusCode(500, ApiResponse<string>.FailResponse("خطا در پردازش داده"));

            return Ok(ApiResponse<DeviceDataDto>.SuccessResponse(data, "داده با موفقیت ذخیره شد."));
        }

        [HttpGet("device/{deviceId}")]
        public async Task<IActionResult> GetDeviceData(string deviceId)
        {
            var data = await _dataProcessorService.GetDeviceDataAsync(deviceId);

            if (data == null || !data.Any())
                return NotFound(ApiResponse<string>.FailResponse("هیچ داده‌ای برای این دستگاه یافت نشد."));

            return Ok(ApiResponse<IEnumerable<DeviceDataDto>>.SuccessResponse(data));
        }

        [HttpGet("device/{deviceId}/latest")]
        public async Task<IActionResult> GetLatestDeviceData(string deviceId)
        {
            var data = await _dataProcessorService.GetLatestDeviceDataAsync(deviceId);

            if (data == null)
                return NotFound(ApiResponse<string>.FailResponse("داده‌ای یافت نشد."));

            return Ok(ApiResponse<DeviceDataDto>.SuccessResponse(data));
        }

        [HttpGet("device/{deviceId}/range")]
        public async Task<IActionResult> GetDataByTimeRange(string deviceId, [FromQuery] DateTime from, [FromQuery] DateTime to)
        {
            var data = await _dataProcessorService.GetDataByTimeRangeAsync(deviceId, from, to);

            if (data == null || !data.Any())
                return NotFound(ApiResponse<string>.FailResponse("داده‌ای در بازه زمانی مشخص یافت نشد."));

            return Ok(ApiResponse<IEnumerable<DeviceDataDto>>.SuccessResponse(data));
        }

        [HttpGet("device/{deviceId}/violations")]
        public async Task<IActionResult> GetViolationData(string deviceId, [FromQuery] string violationType)
        {
            var data = await _dataProcessorService.GetViolationDataAsync(deviceId, violationType);

            if (data == null || !data.Any())
                return NotFound(ApiResponse<string>.FailResponse("داده‌ای با این نوع تخلف یافت نشد."));

            return Ok(ApiResponse<IEnumerable<DeviceDataDto>>.SuccessResponse(data));
        }

        [HttpGet("devices")]
        public async Task<IActionResult> GetAllDevices()
        {
            var devices = await _dataProcessorService.GetAllDevicesAsync();
            return Ok(ApiResponse<IEnumerable<DeviceDto>>.SuccessResponse(devices));
        }

        [HttpGet("device/settings/{deviceId}")]
        public async Task<IActionResult> GetDeviceSettings(string deviceId)
        {
            var settings = await _dataProcessorService.GetDeviceByIdAsync(deviceId);

            if (settings == null)
                return NotFound(ApiResponse<string>.FailResponse("دستگاهی با این شناسه یافت نشد."));

            return Ok(ApiResponse<DeviceSettingsDto>.SuccessResponse(settings));
        }

        [HttpPut("device/settings/{deviceId}")]
        public async Task<IActionResult> UpdateDeviceSettings(string deviceId, [FromBody] DeviceSettingsDto updateDto)
        {
            if (string.IsNullOrEmpty(deviceId))
                return BadRequest(ApiResponse<string>.FailResponse("شناسه دستگاه الزامی است."));

            if (updateDto == null)
                return BadRequest(ApiResponse<string>.FailResponse("اطلاعات به‌روزرسانی نمی‌تواند خالی باشد."));

            var result = await _dataProcessorService.UpdateSettingsAsync(deviceId, updateDto);

            if (!result)
                return BadRequest(ApiResponse<string>.FailResponse("بروزرسانی دستگاه با شکست مواجه شد."));

            return Ok(ApiResponse<string>.SuccessResponse("تنظیمات دستگاه با موفقیت به‌روز شد."));
        }

        [HttpDelete("device/{deviceId}/clear-data")]
        public async Task<IActionResult> DeleteOldData(string deviceId, [FromQuery] DateTime cutoffDate)
        {
            var result = await _dataProcessorService.DeleteOldDataAsync(deviceId, cutoffDate);

            if (!result)
                return BadRequest(ApiResponse<string>.FailResponse("حذف داده‌ها با شکست مواجه شد."));

            return Ok(ApiResponse<string>.SuccessResponse("داده‌های قدیمی با موفقیت حذف شدند."));
        }
    }
}
