using Microsoft.AspNetCore.Mvc;
using DeviceDataProcessor.Services;
using DeviceDataProcessor.DTOs;
using DeviceDataProcessor.Models;  // اضافه کردن namespace ApiResponse
using FluentValidation;
using System.Linq;

namespace DeviceDataProcessor.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IValidator<LoginRequest> _loginValidator;
        private readonly IValidator<RegisterRequest> _registerValidator;

        public AuthController(
            IAuthService authService,
            IValidator<LoginRequest> loginValidator,
            IValidator<RegisterRequest> registerValidator)
        {
            _authService = authService;
            _loginValidator = loginValidator;
            _registerValidator = registerValidator;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var validationResult = await _loginValidator.ValidateAsync(request);
            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
                return BadRequest(ApiResponse<object>.FailResponse(string.Join(", ", errors)));
            }

            var token = await _authService.AuthenticateAsync(request.Username, request.Password);

            if (token == null)
                return Unauthorized(ApiResponse<object>.FailResponse("نام کاربری یا رمز عبور اشتباه است."));

            return Ok(ApiResponse<string>.SuccessResponse(token, "ورود موفقیت‌آمیز بود."));
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            var validationResult = await _registerValidator.ValidateAsync(request);
            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
                return BadRequest(ApiResponse<object>.FailResponse(string.Join(", ", errors)));
            }

            var result = await _authService.RegisterAsync(request.Username, request.Password, request.Role);

            if (!result)
                return BadRequest(ApiResponse<object>.FailResponse("ثبت نام ناموفق بود. نام کاربری ممکن است قبلاً وجود داشته باشد."));

            return Ok(ApiResponse<object>.SuccessResponse(null, "کاربر با موفقیت ثبت شد."));
        }
    }
}
