using FluentValidation;
using DeviceDataProcessor.DTOs;

namespace DeviceDataProcessor.Validators
{
    public class RegisterRequestValidator : AbstractValidator<RegisterRequest>
    {
        public RegisterRequestValidator()
        {
            RuleFor(x => x.Username).NotEmpty().WithMessage("نام کاربری الزامی است.");
            RuleFor(x => x.Password).MinimumLength(6).WithMessage("رمز عبور باید حداقل 6 کاراکتر باشد.");
            RuleFor(x => x.Role).IsInEnum().WithMessage("نقش معتبر نیست.");
        }
    }
}
