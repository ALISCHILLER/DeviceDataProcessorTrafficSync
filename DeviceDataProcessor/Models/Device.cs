using System;
using System.Collections.Generic;

namespace DeviceDataProcessor.Models
{
    /// <summary>
    /// مدل دستگاه – شامل تمامی اطلاعات مربوط به یک دستگاه هوشمند ترافیکی
    /// </summary>
    public class Device
    {
        /// <summary>
        /// شناسه منحصر به فرد دستگاه در دیتابیس
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// شناسه منحصر به فرد دستگاه (مثل شماره سریال)
        /// </summary>
        public string DeviceId { get; set; }

        /// <summary>
        /// نام محور یا محل دستگاه (برای نمایش در جدول)
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// موقعیت جغرافیایی دستگاه (مثلاً "محور تهران - قم")
        /// </summary>
        public string Location { get; set; }

        /// <summary>
        /// وضعیت دستگاه – مشخص کننده اتصال و فعالیت دستگاه
        /// </summary>
        public DeviceStatus Status { get; set; } = DeviceStatus.Offline;

        /// <summary>
        /// زمان آخرین به‌روزرسانی اطلاعات دستگاه
        /// </summary>
        public DateTime LastUpdated { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// FIRST ID – شناسه اولیه دستگاه
        /// </summary>
        public string FID { get; set; }

        /// <summary>
        /// ROAD ID – شناسه محور دستگاه
        /// </summary>
        public string RID { get; set; }

        /// <summary>
        /// مشخص می‌کند که آیا دستگاه در حال حاضر به سرور متصل است
        /// </summary>
        public bool IsConnected { get; set; } = false;

        /// <summary>
        /// زمان آخرین داده‌ای که دستگاه ارسال کرده است
        /// </summary>
        public DateTime? LastSeen { get; set; }

        /// <summary>
        /// داده‌های ثبت شده از این دستگاه
        /// </summary>
        public virtual ICollection<DeviceData> Data { get; set; } = new List<DeviceData>();

        // --- تنظیمات دستگاه ---

        /// <summary>
        /// طول هر یک از لوپ‌ها (Loop Length 1-8)
        /// </summary>
        public List<int> LoopLengths { get; set; } = new List<int>(new int[8]);

        /// <summary>
        /// فاصله بین لوپ‌ها (Loop Distance 1-4)
        /// </summary>
        public List<int> LoopDistances { get; set; } = new List<int>(new int[4]);

        /// <summary>
        /// Offset هر لوپ (Loop Offset 1-8) – دقت اندازه‌گیری
        /// عدد بین 10 تا 1000
        /// </summary>
        public List<int> LoopOffsets { get; set; } = new List<int>(new int[8]);

        /// <summary>
        /// Distance Offset (1-4) – خطای اندازه‌گیری
        /// عدد بین -100 تا +100 (درصد خطا)
        /// </summary>
        public List<int> DistanceOffsets { get; set; } = new List<int>(new int[4]);

        /// <summary>
        /// شناسه محور رفت (Go Road ID)
        /// </summary>
        public string GoRoadId { get; set; }

        /// <summary>
        /// شناسه محور برگشت (Back Road ID)
        /// </summary>
        public string BackRoadId { get; set; }

        /// <summary>
        /// تعداد لاین‌های رفت (حداکثر 8 یا 16 با توجه به نوع کارت)
        /// </summary>
        public int GoLaneCount { get; set; }

        /// <summary>
        /// تعداد لاین‌های برگشت (حداکثر 8 یا 16 با توجه به نوع کارت)
        /// </summary>
        public int BackLaneCount { get; set; }

        /// <summary>
        /// طول ماشین‌های کلاس 1 (سواری و وانت)
        /// </summary>
        public int Class1Length { get; set; }

        /// <summary>
        /// طول ماشین‌های کلاس 2 (کامیونت و مینی‌بوس)
        /// </summary>
        public int Class2Length { get; set; }

        /// <summary>
        /// طول ماشین‌های کلاس 3 (کامیون دو محور)
        /// </summary>
        public int Class3Length { get; set; }

        /// <summary>
        /// طول ماشین‌های کلاس 4 (اتوبوس)
        /// </summary>
        public int Class4Length { get; set; }

        /// <summary>
        /// سرعت مجاز خودروهای سبک در روز
        /// </summary>
        public int LightVehicleSpeedDay { get; set; }

        /// <summary>
        /// سرعت مجاز خودروهای سبک در شب
        /// </summary>
        public int LightVehicleSpeedNight { get; set; }

        /// <summary>
        /// سرعت مجاز خودروهای سنگین در روز
        /// </summary>
        public int HeavyVehicleSpeedDay { get; set; }

        /// <summary>
        /// سرعت مجاز خودروهای سنگین در شب
        /// </summary>
        public int HeavyVehicleSpeedNight { get; set; }

        /// <summary>
        /// ساعت شروع روز (برای تعیین روز/شب)
        /// مثلاً "07:00"
        /// </summary>
        public TimeSpan DayStartTime { get; set; }

        /// <summary>
        /// ساعت پایان روز (برای تعیین روز/شب)
        /// مثلاً "19:00"
        /// </summary>
        public TimeSpan DayEndTime { get; set; }

        /// <summary>
        /// حداقل فاصله زمانی بین خودروها (Gap Time)
        /// واحد: 0.01 ثانیه (عدد بین 10 تا 400)
        /// </summary>
        public int GapTime { get; set; }

        /// <summary>
        /// آدرس IP اول دستگاه
        /// </summary>
        public string Ip1 { get; set; }

        /// <summary>
        /// آدرس IP دوم دستگاه
        /// </summary>
        public string Ip2 { get; set; }

        /// <summary>
        /// آدرس IP سوم دستگاه
        /// </summary>
        public string Ip3 { get; set; }

        /// <summary>
        /// پورت اتصال دستگاه
        /// </summary>
        public int Port { get; set; }

        /// <summary>
        /// زمان خلاصه‌سازی داده‌ها (در واحد دقیقه)
        /// </summary>
        public int SummaryTime { get; set; }

        /// <summary>
        /// شماره تلفن اول برای ارسال پیامک
        /// </summary>
        public string PhoneNumber1 { get; set; }

        /// <summary>
        /// شماره تلفن دوم برای ارسال پیامک
        /// </summary>
        public string PhoneNumber2 { get; set; }

        /// <summary>
        /// وضعیت کارت (فعال / غیرفعال)
        /// </summary>
        public CardState CardState { get; set; } = CardState.Off;

        /// <summary>
        /// جهت کارت (رفت / برگشت)
        /// </summary>
        public CardDirection CardDirection { get; set; } = CardDirection.Go;

        /// <summary>
        /// طول‌های لوپ کارت (اگر کارت متصل باشد)
        /// </summary>
        public List<int> CardLoopLengths { get; set; } = new List<int>(new int[8]);
    }

    /// <summary>
    /// وضعیت دستگاه (Online, Offline)
    /// </summary>
    public enum DeviceStatus
    {
        Online,
        Offline
    }

    /// <summary>
    /// وضعیت کارت (On, Off)
    /// </summary>
    public enum CardState
    {
        On,
        Off
    }

    /// <summary>
    /// جهت کارت (Go, Back)
    /// </summary>
    public enum CardDirection
    {
        Go,
        Back
    }
}