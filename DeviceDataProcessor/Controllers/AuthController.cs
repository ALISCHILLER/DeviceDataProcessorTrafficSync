using Microsoft.AspNetCore.Mvc;
using DeviceDataProcessor.Services;
using DeviceDataProcessor.DTOs;
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
            _authService = authService; // دریافت سرویس احراز هویت
            _loginValidator = loginValidator; // دریافت اعتبارسنجی
        }

        // متد برای ورود کاربر
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequest request)
        {
            var validationResult = await _loginValidator.ValidateAsync(request); // اعتبارسنجی درخواست
            if (!validationResult.IsValid)
                return BadRequest(validationResult.Errors); // در صورت نامعتبر بودن ورودی، خطا برمی‌گرداند

            var token = await _authService.AuthenticateAsync(request.Username, request.Password); // احراز هویت کاربر
            if (token == null)
                return Unauthorized(); // در صورت ناموفق بودن احراز هویت، خطای عدم مجوز برمی‌گرداند

            return Ok(new { Token = token }); // در صورت موفقیت، توکن را برمی‌گرداند
        }

        // متد برای ثبت نام کاربر جدید
        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterRequest request)
        {
            var result = await _authService.RegisterAsync(request.Username, request.Password, request.Role); // ثبت نام کاربر
            if (!result)
                return BadRequest("Registration failed."); // در صورت ناموفق بودن، خطا برمی‌گرداند

            return Ok("User registered successfully."); // در صورت موفقیت، پیام موفقیت برمی‌گرداند
        }
    }
}