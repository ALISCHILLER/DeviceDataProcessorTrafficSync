using FluentValidation;
using DeviceDataProcessor.DTOs;
using DeviceDataProcessor.Models;

namespace DeviceDataProcessor.Validators
{
    /// <summary>
    /// اعتبارسنجی تنظیمات دستگاه – شامل تمامی فیلدهای مرتبط با دستگاه هوشمند ترافیکی
    /// </summary>
    public class DeviceSettingsValidator : AbstractValidator<DeviceSettingsDto>
    {
        public DeviceSettingsValidator()
        {
            // --- تنظیمات عمومی ---
            RuleFor(x => x.Name).NotEmpty().WithMessage("نام دستگاه الزامی است.");
            RuleFor(x => x.Location).NotEmpty().WithMessage("موقعیت دستگاه الزامی است.");
            RuleFor(x => x.FID).NotEmpty().When(x => string.IsNullOrEmpty(x.RID))
                .WithMessage("FID الزامی است اگر RID وجود نداشته باشد.");
            RuleFor(x => x.RID).NotEmpty().When(x => string.IsNullOrEmpty(x.FID))
                .WithMessage("RID الزامی است اگر FID وجود نداشته باشد.");

            // --- لوپ‌ها ---
            RuleFor(x => x.LoopLengths).Must(BeValidLoopLengths)
                .WithMessage("طول لوپ باید بین 10 تا 1000 باشد.")
                .When(x => x.LoopLengths != null);

            RuleFor(x => x.LoopDistances).Must(BeValidLoopDistances)
                .WithMessage("فاصله لوپ باید مثبت باشد.")
                .When(x => x.LoopDistances != null);

            RuleFor(x => x.LoopOffsets).Must(BeValidLoopOffsets)
                .WithMessage("Offset لوپ باید بین 10 تا 1000 باشد.")
                .When(x => x.LoopOffsets != null);

            RuleFor(x => x.DistanceOffsets).Must(BeValidDistanceOffsets)
                .WithMessage("خطای اندازه‌گیری فاصله باید بین -100 تا +100 باشد.")
                .When(x => x.DistanceOffsets != null);

            // --- کلاس خودروها ---
            RuleFor(x => x.Class1Length).InclusiveBetween(500, 600)
                .WithMessage("طول کلاس 1 باید بین 500 تا 600 باشد.");
            RuleFor(x => x.Class2Length).GreaterThan(x => x.Class1Length)
                .WithMessage("طول کلاس 2 باید بزرگتر از طول کلاس 1 باشد.");
            RuleFor(x => x.Class3Length).GreaterThan(x => x.Class2Length)
                .WithMessage("طول کلاس 3 باید بزرگتر از طول کلاس 2 باشد.");
            RuleFor(x => x.Class4Length).GreaterThan(x => x.Class3Length)
                .WithMessage("طول کلاس 4 باید بزرگتر از طول کلاس 3 باشد.");

            // --- سرعت مجاز ---
            RuleFor(x => x.LightVehicleSpeedDay).InclusiveBetween(0, 150)
                .WithMessage("سرعت مجاز خودروهای سبک در روز باید بین 0 تا 150 باشد.");
            RuleFor(x => x.LightVehicleSpeedNight).InclusiveBetween(0, 120)
                .WithMessage("سرعت مجاز خودروهای سبک در شب باید بین 0 تا 120 باشد.");
            RuleFor(x => x.HeavyVehicleSpeedDay).InclusiveBetween(0, 120)
                .WithMessage("سرعت مجاز خودروهای سنگین در روز باید بین 0 تا 120 باشد.");
            RuleFor(x => x.HeavyVehicleSpeedNight).InclusiveBetween(0, 100)
                .WithMessage("سرعت مجاز خودروهای سنگین در شب باید بین 0 تا 100 باشد.");

            // --- ساعات روز / شب ---
            RuleFor(x => x.DayStartTime).LessThan(x => x.DayEndTime)
                .WithMessage("زمان شروع باید قبل از زمان پایان باشد.");
            RuleFor(x => x.DayEndTime).GreaterThan(x => x.DayStartTime)
                .WithMessage("زمان پایان باید بعد از زمان شروع باشد.");

            // --- تعداد لاین‌ها ---
            RuleFor(x => x.GoLaneCount + x.BackLaneCount)
                .Custom((total, context) =>
                {
                    var dto = context.InstanceToValidate as DeviceSettingsDto;
                    if (dto == null) return;

                    if (dto.CardState == CardState.On && total > 16)
                        context.AddFailure("جمع لاین‌ها وقتی کارت روشن است نمی‌تواند بیشتر از 16 باشد.");
                    else if (dto.CardState == CardState.Off && total > 8)
                        context.AddFailure("جمع لاین‌ها وقتی کارت خاموش است نمی‌تواند بیشتر از 8 باشد.");
                });

            // --- فاصله زمانی بین خودروها ---
            RuleFor(x => x.GapTime).InclusiveBetween(10, 400)
                .WithMessage("فاصله زمانی باید بین 10 تا 400 باشد.");

            // --- IPها ---
            RuleFor(x => x.Ip1).NotEmpty().When(x => string.IsNullOrEmpty(x.Ip2) && string.IsNullOrEmpty(x.Ip3))
                .WithMessage("حداقل یک آدرس IP الزامی است.");

            RuleFor(x => x.Port).InclusiveBetween(1, 65535)
                .WithMessage("پورت باید عددی بین 1 تا 65535 باشد.");

            RuleFor(x => x.SummaryTime).GreaterThanOrEqualTo(1)
                .WithMessage("زمان خلاصه‌سازی باید حداقل 1 دقیقه باشد.");

            // --- شماره تلفن ---
            RuleFor(x => x.PhoneNumber1).Matches(@"^(\+98|0)?\d{10}$")
                .When(x => !string.IsNullOrEmpty(x.PhoneNumber1))
                .WithMessage("شماره تلفن 1 معتبر نیست.");
            RuleFor(x => x.PhoneNumber2).Matches(@"^(\+98|0)?\d{10}$")
                .When(x => !string.IsNullOrEmpty(x.PhoneNumber2))
                .WithMessage("شماره تلفن 2 معتبر نیست.");

            // --- وضعیت کارت ---
            RuleFor(x => x.CardState).IsInEnum()
                .WithMessage("وضعیت کارت معتبر نیست.");
            RuleFor(x => x.CardDirection).IsInEnum()
                .WithMessage("جهت کارت معتبر نیست.");
        }

        private bool BeValidLoopLengths(List<int> values) => values.All(x => x >= 10 && x <= 1000);
        private bool BeValidLoopDistances(List<int> values) => values.All(x => x >= 0);
        private bool BeValidLoopOffsets(List<int> values) => values.All(x => x >= 10 && x <= 1000);
        private bool BeValidDistanceOffsets(List<int> values) => values.All(x => x >= -100 && x <= 100);
    }
}