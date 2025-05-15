using FluentValidation;
using DeviceDataProcessor.DTOs;

namespace DeviceDataProcessor.Validators
{
    public class DeviceDataDtoValidator : AbstractValidator<DeviceDataDto>
    {
        public DeviceDataDtoValidator()
        {
            RuleFor(x => x.DeviceId).NotEmpty().WithMessage("شناسه دستگاه الزامی است.");
            RuleFor(x => x.Timestamp).NotNull().WithMessage("زمان ثبت داده الزامی است.");

            RuleFor(x => x.ST).NotNull().WithMessage("ST (زمان شروع) الزامی است.");
            RuleFor(x => x.ET).GreaterThan(x => x.ST).WithMessage("ET (زمان پایان) باید بعد از ST باشد.");

            RuleFor(x => x.C1).GreaterThanOrEqualTo(0);
            RuleFor(x => x.C2).GreaterThanOrEqualTo(0);
            RuleFor(x => x.C3).GreaterThanOrEqualTo(0);
            RuleFor(x => x.C4).GreaterThanOrEqualTo(0);
            RuleFor(x => x.C5).GreaterThanOrEqualTo(0);

            RuleFor(x => x.ASP).GreaterThan(0).When(x => x.ASP.HasValue)
                .WithMessage("سرعت متوسط (ASP) باید بیشتر از صفر باشد.");

            RuleFor(x => x.SO).GreaterThanOrEqualTo(0).When(x => x.SO.HasValue)
                .WithMessage("SO (تخلف سرعت) باید بیشتر یا مساوی صفر باشد.");

            RuleFor(x => x.OO).GreaterThanOrEqualTo(0).When(x => x.OO.HasValue)
                .WithMessage("OO (تخلف سبقت) باید بیشتر یا مساوی صفر باشد.");

            RuleFor(x => x.ESD).GreaterThanOrEqualTo(0).When(x => x.ESD.HasValue)
                .WithMessage("ESD (فاصله غیرمجاز) باید بیشتر یا مساوی صفر باشد.");
        }
    }
}