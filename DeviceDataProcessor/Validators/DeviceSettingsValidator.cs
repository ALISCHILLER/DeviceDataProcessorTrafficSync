using FluentValidation;
using DeviceDataProcessor.DTOs;
using DeviceDataProcessor.Models;

namespace DeviceDataProcessor.Validators
{
    public class DeviceSettingsValidator : AbstractValidator<DeviceSettingsDto>
    {
        public DeviceSettingsValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("نام دستگاه الزامی است.");
            RuleFor(x => x.Location).NotEmpty().WithMessage("موقعیت دستگاه الزامی است.");
            RuleFor(x => x.FID).NotEmpty().When(x => string.IsNullOrEmpty(x.RID))
                .WithMessage("FID الزامی است اگر RID وجود نداشته باشد.");

            // لوپ‌ها
            RuleFor(x => x.LoopLengths).Must(BeValidLoopLengths)
                .WithMessage("طول لوپ باید بین 10 تا 1000 باشد.")
                .When(x => x.LoopLengths != null);

            RuleFor(x => x.LoopDistances).Must(BeValidLoopDistances)
                .WithMessage("فاصله لوپ باید مثبت باشد.")
                .When(x => x.LoopDistances != null);

            // کلاس‌ها
            RuleFor(x => x.Class1Length).GreaterThan(0);
            RuleFor(x => x.Class2Length).GreaterThan(x => x.Class1Length);
            RuleFor(x => x.Class3Length).GreaterThan(x => x.Class2Length);
            RuleFor(x => x.Class4Length).GreaterThan(x => x.Class3Length);

            // سرعت مجاز
            RuleFor(x => x.LightVehicleSpeedDay).InclusiveBetween(0, 150);
            RuleFor(x => x.LightVehicleSpeedNight).InclusiveBetween(0, 150);
            RuleFor(x => x.HeavyVehicleSpeedDay).InclusiveBetween(0, 120);
            RuleFor(x => x.HeavyVehicleSpeedNight).InclusiveBetween(0, 120);

            // ساعات روز / شب
            RuleFor(x => x.DayStartTime).LessThan(x => x.DayEndTime);
            RuleFor(x => x.DayEndTime).GreaterThan(x => x.DayStartTime);

            // جمع لاین‌ها
            RuleFor(x => x.GoLaneCount + x.BackLaneCount)
                .Custom((total, context) =>
                {
                    var dto = context.InstanceToValidate as DeviceSettingsDto;
                    if (dto.CardState == CardState.On && total > 16)
                        context.AddFailure("جمع لاین‌ها وقتی کارت روشن است نمی‌تواند بیشتر از 16 باشد.");
                    else if (dto.CardState == CardState.Off && total > 8)
                        context.AddFailure("جمع لاین‌ها وقتی کارت خاموش است نمی‌تواند بیشتر از 8 باشد.");
                });

            // فاصله زمانی
            RuleFor(x => x.GapTime).InclusiveBetween(10, 400)
                .WithMessage("فاصله زمانی باید بین 10 تا 400 باشد.");
        }

        private bool BeValidLoopLengths(List<int> values)
        {
            return values.All(x => x >= 10 && x <= 1000);
        }

        private bool BeValidLoopDistances(List<int> values)
        {
            return values.All(x => x >= 0);
        }
    }
}