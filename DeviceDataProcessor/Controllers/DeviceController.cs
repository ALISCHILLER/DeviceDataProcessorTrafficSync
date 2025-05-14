using DeviceDataProcessor.Data;
using DeviceDataProcessor.DTOs;
using DeviceDataProcessor.Models;
using Microsoft.AspNetCore.Mvc;

namespace DeviceDataProcessor.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DeviceController : ControllerBase
    {
        private readonly IUserRepository<Device> _deviceRepo;

        public DeviceController(IUserRepository<Device> deviceRepo) => _deviceRepo = deviceRepo;

        [HttpGet]
        public async Task<IActionResult> GetAll() => Ok(await _deviceRepo.GetAllAsync());

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var device = await _deviceRepo.GetByIdAsync(id);
            return device == null ? NotFound() : Ok(device);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] DeviceDto dto)
        {
            var device = new Device
            {
                Name = dto.Name,
                FID = dto.FID,
                RID = dto.RID
            };
            await _deviceRepo.AddAsync(device);
            return CreatedAtAction(nameof(Get), new { id = device.Id }, device);
        }
    }
}
