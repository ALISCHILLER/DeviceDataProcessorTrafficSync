using FluentValidation;
using DeviceDataProcessor.DTOs;

namespace DeviceDataProcessor.Validators
{
    public class LoginRequestValidator : AbstractValidator<LoginRequest>
    {
        public LoginRequestValidator()
        {
            RuleFor(x => x.Username).NotEmpty().WithMessage("Username is required."); // اعتبارسنجی نام کاربری
            RuleFor(x => x.Password).NotEmpty().WithMessage("Password is required."); // اعتبارسنجی پسورد
        }
    }
}