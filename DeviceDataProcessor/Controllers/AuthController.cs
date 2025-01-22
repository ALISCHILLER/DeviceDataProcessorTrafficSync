using Microsoft.AspNetCore.Mvc;
using DeviceDataProcessor.Services;
using DeviceDataProcessor.DTOs;
using DeviceDataProcessor.Validators;
using FluentValidation;

namespace DeviceDataProcessor.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService; // سرویس احراز هویت
        private readonly IValidator<LoginRequest> _loginValidator; // اعتبارسنجی درخواست ورود

        public AuthController(IAuthService authService, IValidator<LoginRequest> loginValidator)
        {
            _authService = authService;
            _loginValidator = loginValidator;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequest request)
        {
            var validationResult = await _loginValidator.ValidateAsync(request);
            if (!validationResult.IsValid)
                return BadRequest(validationResult.Errors); // در صورت نامعتبر بودن ورودی، خطا برمی‌گرداند

            var token = await _authService.AuthenticateAsync(request.Username, request.Password);
            if (token == null)
                return Unauthorized(); // در صورت ناموفق بودن احراز هویت، خطای عدم مجوز برمی‌گرداند

            return Ok(new { Token = token }); // در صورت موفقیت، توکن را برمی‌گرداند
        }
    }
}