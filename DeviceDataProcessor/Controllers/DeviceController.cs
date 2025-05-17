using Microsoft.AspNetCore.Mvc;
using DeviceDataProcessor.Services;
using DeviceDataProcessor.DTOs;
using System.Threading.Tasks;
using DeviceDataProcessor.Models;
using System;
using System.Linq;

namespace DeviceDataProcessor.Controllers
{
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

        [HttpGet]
        public async Task<IActionResult> GetAllDevices()
        {
            var devices = await _deviceService.GetAllDevicesAsync();
            return Ok(ApiResponse<object>.SuccessResponse(devices));
        }

        [HttpGet("{deviceId}")]
        public async Task<IActionResult> GetDevice(string deviceId)
        {
            var device = await _deviceService.GetDeviceByIdAsync(deviceId);
            if (device == null)
                return NotFound(ApiResponse<object>.FailResponse("دستگاهی با این شناسه یافت نشد."));

            return Ok(ApiResponse<object>.SuccessResponse(device));
        }

        // --- تنظیمات دستگاه ---

        [HttpGet("{deviceId}/settings")]
        public async Task<IActionResult> GetDeviceSettings(string deviceId)
        {
            var settings = await _deviceService.GetSettingsByDeviceIdAsync(deviceId);
            if (settings == null)
                return NotFound(ApiResponse<object>.FailResponse("تنظیمات دستگاه یافت نشد."));

            return Ok(ApiResponse<object>.SuccessResponse(settings));
        }

        [HttpPut("{deviceId}/settings")]
        public async Task<IActionResult> UpdateDeviceSettings(string deviceId, [FromBody] DeviceSettingsDto updateDto)
        {
            if (updateDto == null)
                return BadRequest(ApiResponse<object>.FailResponse("داده‌های به‌روزرسانی نمی‌تواند خالی باشد."));

            var result = await _deviceService.UpdateSettingsAsync(deviceId, updateDto);

            if (!result)
                return BadRequest(ApiResponse<object>.FailResponse("بروزرسانی دستگاه با شکست مواجه شد."));

            return Ok(ApiResponse<object>.SuccessResponse(null, "تنظیمات دستگاه با موفقیت به‌روز شد."));
        }

        [HttpPut("{deviceId}/status")]
        public async Task<IActionResult> UpdateDeviceStatus(string deviceId, [FromBody] string status)
        {
            var isValidStatus = Enum.TryParse<DeviceStatus>(status, true, out var parsedStatus);
            if (!isValidStatus)
                return BadRequest(ApiResponse<object>.FailResponse("وضعیت نامعتبر. فقط 'Online' یا 'Offline' قابل قبول است."));

            var device = await _deviceService.GetDeviceByIdAsync(deviceId);
            if (device == null)
                return NotFound(ApiResponse<object>.FailResponse("دستگاهی با این شناسه یافت نشد."));

            var updateDto = new DeviceUpdateDto { Status = parsedStatus };
            var result = await _deviceService.UpdateDeviceAsync(deviceId, updateDto);

            if (!result)
                return BadRequest(ApiResponse<object>.FailResponse("به‌روزرسانی وضعیت دستگاه با شکست مواجه شد."));

            return Ok(ApiResponse<object>.SuccessResponse(null, "وضعیت دستگاه با موفقیت به‌روز شد."));
        }

        // --- داده‌های دستگاه ---

        [HttpGet("{deviceId}/data")]
        public async Task<IActionResult> GetDeviceData(string deviceId, [FromQuery] DateTime from, [FromQuery] DateTime to)
        {
            var data = await _deviceService.GetDeviceDataByTimeRangeAsync(deviceId, from, to);

            if (data == null || !data.Any())
                return NotFound(ApiResponse<object>.FailResponse("داده‌ای در بازه زمانی مشخص یافت نشد."));

            return Ok(ApiResponse<object>.SuccessResponse(data));
        }

        [HttpGet("{deviceId}/data/latest")]
        public async Task<IActionResult> GetLatestDeviceData(string deviceId)
        {
            var data = await _deviceService.GetLatestDeviceDataAsync(deviceId);
            if (data == null)
                return NotFound(ApiResponse<object>.FailResponse("داده‌ای برای این دستگاه یافت نشد."));

            return Ok(ApiResponse<object>.SuccessResponse(data));
        }

        [HttpGet("{deviceId}/data/violations")]
        public async Task<IActionResult> GetViolationData(string deviceId, [FromQuery] string violationType)
        {
            var data = await _deviceService.GetViolationDataAsync(deviceId, violationType);

            if (data == null || !data.Any())
                return NotFound(ApiResponse<object>.FailResponse("داده‌ای با این نوع تخلف یافت نشد."));

            return Ok(ApiResponse<object>.SuccessResponse(data));
        }

        [HttpDelete("{deviceId}/data/clear")]
        public async Task<IActionResult> DeleteOldData(string deviceId, [FromQuery] DateTime cutoffDate)
        {
            var result = await _deviceService.DeleteOldDataAsync(deviceId, cutoffDate);

            if (!result)
                return BadRequest(ApiResponse<object>.FailResponse("حذف داده‌ها با شکست مواجه شد."));

            return Ok(ApiResponse<object>.SuccessResponse(null, "داده‌های قدیمی با موفقیت حذف شدند."));
        }
    }
}
