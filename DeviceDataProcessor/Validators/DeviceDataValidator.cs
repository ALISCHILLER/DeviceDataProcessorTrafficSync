using FluentValidation;
using DeviceDataProcessor.DTOs;

namespace DeviceDataProcessor.Validators
{
    public class DeviceDataValidator : AbstractValidator<DeviceDataDto>
    {
        public DeviceDataValidator()
        {
            // اعتبارسنجی فیلدهای الزامی
            RuleFor(x => x.DeviceId).NotEmpty().WithMessage("شناسه دستگاه الزامی است.");
            RuleFor(x => x.ST).NotNull().WithMessage("زمان شروع (ST) الزامی است.");
            RuleFor(x => x.ET).NotNull().WithMessage("زمان پایان (ET) الزامی است.");

            // اعتبارسنجی کلاس‌ها (باید ≥ 0 باشند)
            RuleFor(x => x.C1).GreaterThanOrEqualTo(0).WithMessage("C1 باید بزرگتر یا مساوی صفر باشد.");
            RuleFor(x => x.C2).GreaterThanOrEqualTo(0).WithMessage("C2 باید بزرگتر یا مساوی صفر باشد.");
            RuleFor(x => x.C3).GreaterThanOrEqualTo(0).WithMessage("C3 باید بزرگتر یا مساوی صفر باشد.");
            RuleFor(x => x.C4).GreaterThanOrEqualTo(0).WithMessage("C4 باید بزرگتر یا مساوی صفر باشد.");
            RuleFor(x => x.C5).GreaterThanOrEqualTo(0).WithMessage("C5 باید بزرگتر یا مساوی صفر باشد.");

            // اعتبارسنجی مقادیر اختیاری
            RuleFor(x => x.ASP).GreaterThan(0).When(x => x.ASP.HasValue)
                .WithMessage("سرعت متوسط (ASP) باید بیشتر از صفر باشد.");

            RuleFor(x => x.SO).GreaterThanOrEqualTo(0).When(x => x.SO.HasValue)
                .WithMessage("تخلف سرعت (SO) باید بیشتر یا مساوی صفر باشد.");

            RuleFor(x => x.OO).GreaterThanOrEqualTo(0).When(x => x.OO.HasValue)
                .WithMessage("تخلف سبقت (OO) باید بیشتر یا مساوی صفر باشد.");

            RuleFor(x => x.ESD).GreaterThanOrEqualTo(0).When(x => x.ESD.HasValue)
                .WithMessage("تخلف فاصله (ESD) باید بیشتر یا مساوی صفر باشد.");
        }
    }
}