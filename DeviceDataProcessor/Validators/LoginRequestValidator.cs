using FluentValidation;
using DeviceDataProcessor.DTOs;

namespace DeviceDataProcessor.Validators
{
    /// <summary>
    /// اعتبارسنجی درخواست ورود کاربران
    /// </summary>
    public class LoginRequestValidator : AbstractValidator<LoginRequest>
    {
        public LoginRequestValidator()
        {
            RuleFor(x => x.Username)
                .NotEmpty().WithMessage("نام کاربری الزامی است.")
                .MaximumLength(50).WithMessage("نام کاربری نباید بیشتر از 50 کاراکتر باشد.");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("رمز عبور الزامی است.")
                .MinimumLength(6).WithMessage("رمز عبور باید حداقل 6 کاراکتر باشد.")
                .MaximumLength(100).WithMessage("رمز عبور نباید بیشتر از 100 کاراکتر باشد.");
        }
    }
}
