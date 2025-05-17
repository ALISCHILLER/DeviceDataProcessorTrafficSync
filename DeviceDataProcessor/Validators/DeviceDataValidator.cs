using FluentValidation;
using DeviceDataProcessor.DTOs;

namespace DeviceDataProcessor.Validators
{
    /// <summary>
    /// اعتبارسنجی داده‌های ورودی DeviceDataDto
    /// </summary>
    public class DeviceDataDtoValidator : AbstractValidator<DeviceDataDto>
    {
        public DeviceDataDtoValidator()
        {
            // اعتبارسنجی شناسه دستگاه: نباید خالی باشد
            RuleFor(x => x.DeviceId)
                .NotEmpty()
                .WithMessage("شناسه دستگاه الزامی است.");

            // اعتبارسنجی زمان ثبت داده: نباید خالی باشد
            RuleFor(x => x.Timestamp)
                .NotNull()
                .WithMessage("زمان ثبت داده الزامی است.");

            // اعتبارسنجی زمان شروع (ST): نباید خالی باشد
            RuleFor(x => x.ST)
                .NotNull()
                .WithMessage("ST (زمان شروع) الزامی است.");

            // اعتبارسنجی زمان پایان (ET) نسبت به ST
            RuleFor(x => x)
                .Must(dto =>
                {
                    // محاسبه اختلاف زمان ET و ST به ثانیه
                    var diff = (dto.ET - dto.ST).TotalSeconds;

                    // اگر ET کوچکتر از ST بود، یعنی مربوط به روز بعد است، پس 24 ساعت را اضافه می‌کنیم
                    if (diff < 0) diff += 24 * 3600;

                    // شرط: فاصله زمانی باید بیشتر از صفر و کمتر یا مساوی 24 ساعت باشد
                    return diff > 0 && diff <= 24 * 3600;
                })
                .WithMessage("ET (زمان پایان) باید بعد از ST باشد و فاصله آن‌ها کمتر از ۲۴ ساعت باشد.");

            // اعتبارسنجی تعداد جریان کلاس‌ها باید صفر یا مثبت باشد
            RuleFor(x => x.C1)
                .GreaterThanOrEqualTo(0)
                .WithMessage("C1 نمی‌تواند منفی باشد.");
            RuleFor(x => x.C2)
                .GreaterThanOrEqualTo(0)
                .WithMessage("C2 نمی‌تواند منفی باشد.");
            RuleFor(x => x.C3)
                .GreaterThanOrEqualTo(0)
                .WithMessage("C3 نمی‌تواند منفی باشد.");
            RuleFor(x => x.C4)
                .GreaterThanOrEqualTo(0)
                .WithMessage("C4 نمی‌تواند منفی باشد.");
            RuleFor(x => x.C5)
                .GreaterThanOrEqualTo(0)
                .WithMessage("C5 نمی‌تواند منفی باشد.");

            // اعتبارسنجی سرعت متوسط (ASP) اگر مقدار داشته باشد، باید بزرگتر از صفر باشد
            RuleFor(x => x.ASP)
                .GreaterThan(0)
                .When(x => x.ASP.HasValue)
                .WithMessage("سرعت متوسط (ASP) باید بیشتر از صفر باشد.");

            // اعتبارسنجی تخلفات سرعت (SO) اگر مقدار داشته باشد، باید صفر یا بیشتر باشد
            RuleFor(x => x.SO)
                .GreaterThanOrEqualTo(0)
                .When(x => x.SO.HasValue)
                .WithMessage("SO (تخلف سرعت) باید بیشتر یا مساوی صفر باشد.");

            // اعتبارسنجی تخلفات سبقت (OO) اگر مقدار داشته باشد، باید صفر یا بیشتر باشد
            RuleFor(x => x.OO)
                .GreaterThanOrEqualTo(0)
                .When(x => x.OO.HasValue)
                .WithMessage("OO (تخلف سبقت) باید بیشتر یا مساوی صفر باشد.");

            // اعتبارسنجی تخلفات فاصله غیرمجاز (ESD) اگر مقدار داشته باشد، باید صفر یا بیشتر باشد
            RuleFor(x => x.ESD)
                .GreaterThanOrEqualTo(0)
                .When(x => x.ESD.HasValue)
                .WithMessage("ESD (فاصله غیرمجاز) باید بیشتر یا مساوی صفر باشد.");

            // اعتبارسنجی مقدار اصلی داده (Value) اگر مقدار داشته باشد، باید صفر یا بیشتر باشد
            RuleFor(x => x.Value)
                .GreaterThanOrEqualTo(0)
                .When(x => x.Value.HasValue)
                .WithMessage("Value باید صفر یا بیشتر باشد.");

            // اعتبارسنجی کیفیت داده (Quality) اگر مقدار داشته باشد، باید از enum معتبر باشد
            RuleFor(x => x.Quality)
                .IsInEnum()
                .When(x => x.Quality.HasValue)
                .WithMessage("Quality نامعتبر است.");
        }
    }
}
