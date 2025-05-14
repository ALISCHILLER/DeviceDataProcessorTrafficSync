using FluentValidation;
using DeviceDataProcessor.DTOs;

namespace DeviceDataProcessor.Validators
{
    // اعتبارسنجی درخواست ورود
    public class LoginRequestValidator : AbstractValidator<LoginRequest>
    {
        public LoginRequestValidator()
        {
            RuleFor(x => x.Username).NotEmpty().WithMessage("نام کاربری الزامی است."); // اعتبارسنجی نام کاربری
            RuleFor(x => x.Password).NotEmpty().WithMessage("رمز عبور الزامی است."); // اعتبارسنجی پسورد
        }
    }
}