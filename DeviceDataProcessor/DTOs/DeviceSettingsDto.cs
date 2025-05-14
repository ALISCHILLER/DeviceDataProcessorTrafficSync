using DeviceDataProcessor.Models;
using System;
using System.Collections.Generic;

namespace DeviceDataProcessor.DTOs
{
    /// <summary>
    /// DTO برای تنظیمات کامل دستگاه
    /// </summary>
    public class DeviceSettingsDto
    {
        // --- تنظیمات عمومی ---
        public string Name { get; set; } // اسم محور
        public string Location { get; set; } // موقعیت دستگاه
        public string FID { get; set; } // FIRST ID
        public string RID { get; set; } // ROAD ID

        // --- تنظیمات لوپ ---
        /// <summary>
        /// طول هر یک از لوپ‌ها (Loop Length 1-8)
        /// عدد بین 10 تا 1000
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
        /// خطای اندازه‌گیری فاصله (Distance Offset 1-4)
        /// عدد بین -100 تا +100 (درصد)
        /// </summary>
        public List<int> DistanceOffsets { get; set; } = new List<int>(new int[4]);

        // --- تنظیمات محور ---
        public string GoRoadId { get; set; } // شناسه محور رفت
        public string BackRoadId { get; set; } // شناسه محور برگشت
        public int GoLaneCount { get; set; } // تعداد لاین رفت
        public int BackLaneCount { get; set; } // تعداد لاین برگشت

        // --- تنظیمات کلاس خودرو ---
        public int Class1Length { get; set; } // طول کلاس 1 (سواری، وانت)
        public int Class2Length { get; set; } // طول کلاس 2 (کامیونت، مینی‌بوس)
        public int Class3Length { get; set; } // طول کلاس 3 (کامیون دو محور)
        public int Class4Length { get; set; } // طول کلاس 4 (اتوبوس)

        // --- تنظیمات تخلفات ---
        public int LightVehicleSpeedDay { get; set; } // سرعت مجاز خودروهای سبک در روز
        public int LightVehicleSpeedNight { get; set; } // سرعت مجاز خودروهای سبک در شب
        public int HeavyVehicleSpeedDay { get; set; } // سرعت مجاز خودروهای سنگین در روز
        public int HeavyVehicleSpeedNight { get; set; } // سرعت مجاز خودروهای سنگین در شب
        public TimeSpan DayStartTime { get; set; } // ساعت شروع روز
        public TimeSpan DayEndTime { get; set; } // ساعت پایان روز
        public int GapTime { get; set; } // حداقل فاصله زمانی بین خودروها (عدد 10-400)

        // --- تنظیمات شبکه ---
        public string Ip1 { get; set; } // آدرس IP اول
        public string Ip2 { get; set; } // آدرس IP دوم
        public string Ip3 { get; set; } // آدرس IP سوم
        public int Port { get; set; } // پورت سرور
        public int SummaryTime { get; set; } // زمان ارسال اطلاعات (دقیقه)
        public string PhoneNumber1 { get; set; } // شماره تلفن 1
        public string PhoneNumber2 { get; set; } // شماره تلفن 2

        // --- تنظیمات کارت ---
        public CardState CardState { get; set; } = CardState.Off; // وضعیت کارت (On/Off)
        public CardDirection CardDirection { get; set; } = CardDirection.Go; // جهت کارت (Go/Back)
        public List<int> CardLoopLengths { get; set; } = new List<int>(new int[8]); // طول لوپ‌های کارت
    }
}