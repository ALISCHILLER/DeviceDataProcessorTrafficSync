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
        // سرویس احراز هویت
        private readonly IAuthService _authService;

        // اعتبارسنجی درخواست ورود
        private readonly IValidator<LoginRequest> _loginValidator;

        // اعتبارسنجی درخواست ثبت نام
        private readonly IValidator<RegisterRequest> _registerValidator;

        // سازنده کلاس - Dependency Injection
        public AuthController(
            IAuthService authService,
            IValidator<LoginRequest> loginValidator,
            IValidator<RegisterRequest> registerValidator)
        {
            _authService = authService;
            _loginValidator = loginValidator;
            _registerValidator = registerValidator;
        }

        /// <summary>
        /// متد ورود کاربر
        /// </summary>
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequest request)
        {
            // اعتبارسنجی ورودی با FluentValidation
            var validationResult = await _loginValidator.ValidateAsync(request);
            if (!validationResult.IsValid)
                return BadRequest(validationResult.Errors); // خطا در ورودی

            // احراز هویت کاربر
            var token = await _authService.AuthenticateAsync(request.Username, request.Password);

            if (token == null)
                return Unauthorized(); // نام کاربری یا رمز اشتباه است

            return Ok(new { Token = token }); // توکن JWT به کاربر داده می‌شود
        }

        /// <summary>
        /// متد ثبت نام کاربر جدید
        /// </summary>
        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterRequest request)
        {
            // اعتبارسنجی ورودی با FluentValidation
            var validationResult = await _registerValidator.ValidateAsync(request);
            if (!validationResult.IsValid)
                return BadRequest(validationResult.Errors); // خطا در ورودی

            // ثبت کاربر جدید
            var result = await _authService.RegisterAsync(request.Username, request.Password, request.Role);

            if (!result)
                return BadRequest("ثبت نام ناموفق بود."); // کاربر قبلاً وجود دارد

            return Ok("کاربر با موفقیت ثبت شد."); // ثبت موفق
        }
    }
}