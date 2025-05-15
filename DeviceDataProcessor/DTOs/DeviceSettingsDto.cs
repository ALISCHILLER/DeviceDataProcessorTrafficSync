using DeviceDataProcessor.Models;
using System.Collections.Generic;

namespace DeviceDataProcessor.DTOs
{
    /// <summary>
    /// DTO برای تنظیمات کامل دستگاه – شامل تمام فیلدهای موجود در فایل PDF
    /// </summary>
    public class DeviceSettingsDto
    {
        // --- عمومی ---
        public string Name { get; set; } // اسم محور
        public string Location { get; set; } // موقعیت دستگاه
        public string Status { get; set; } // وضعیت دستگاه (Online/Offline)
        public string FID { get; set; } // FIRST ID
        public string RID { get; set; } // ROAD ID

        // --- لوپ ---
        public List<int> LoopLengths { get; set; } = new List<int>(new int[8]); // Loop Length 1-8
        public List<int> LoopDistances { get; set; } = new List<int>(new int[4]); // Loop Distance 1-4
        public List<int> LoopOffsets { get; set; } = new List<int>(new int[8]); // Loop Offset 1-8
        public List<int> DistanceOffsets { get; set; } = new List<int>(new int[4]); // Distance Offset 1-4

        // --- محور ---
        public string GoRoadId { get; set; } // شناسه محور رفت
        public string BackRoadId { get; set; } // شناسه محور برگشت
        public int GoLaneCount { get; set; } // تعداد لاین رفت
        public int BackLaneCount { get; set; } // تعداد لاین برگشت

        // --- کلاس خودروها ---
        public int Class1Length { get; set; } // طول کلاس 1 (سواری، وانت)
        public int Class2Length { get; set; } // طول کلاس 2 (کامیونت، مینی‌بوس)
        public int Class3Length { get; set; } // طول کلاس 3 (کامیون دو محور)
        public int Class4Length { get; set; } // طول کلاس 4 (اتوبوس)

        // --- تخلفات ---
        public int LightVehicleSpeedDay { get; set; } // سرعت مجاز خودروهای سبک در روز
        public int LightVehicleSpeedNight { get; set; } // سرعت مجاز خودروهای سبک در شب
        public int HeavyVehicleSpeedDay { get; set; } // سرعت مجاز خودروهای سنگین در روز
        public int HeavyVehicleSpeedNight { get; set; } // سرعت مجاز خودروهای سنگین در شب
        public TimeSpan DayStartTime { get; set; } // ساعت شروع روز
        public TimeSpan DayEndTime { get; set; } // ساعت پایان روز
        public int GapTime { get; set; } // حداقل فاصله زمانی بین خودروها (عدد 10-400)

        // --- شبکه ---
        public string Ip1 { get; set; } // آدرس IP اول
        public string Ip2 { get; set; } // آدرس IP دوم
        public string Ip3 { get; set; } // آدرس IP سوم
        public int Port { get; set; } // پورت دستگاه
        public int SummaryTime { get; set; } // زمان خلاصه‌سازی داده (دقیقه)
        public string PhoneNumber1 { get; set; } // شماره تماس 1
        public string PhoneNumber2 { get; set; } // شماره تماس 2

        // --- کارت ---
        public CardState CardState { get; set; } = CardState.Off; // وضعیت کارت (On/Off)
        public CardDirection CardDirection { get; set; } = CardDirection.Go; // جهت کارت (Go/Back)
        public List<int> CardLoopLengths { get; set; } = new List<int>(new int[8]); // طول لوپ‌های کارت
    }
}