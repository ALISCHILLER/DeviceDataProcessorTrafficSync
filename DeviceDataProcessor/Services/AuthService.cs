using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

// فضاهای نام لازم برای پروژه
using DeviceDataProcessor.Models;         // مدل کاربر
using DeviceDataProcessor.Data;           // رابط IRepository و دسترسی به داده‌ها
using DeviceDataProcessor.Settings;       // تنظیمات JWT
using Microsoft.Extensions.Options;       // برای خواندن تنظیمات از appsettings.json
using Microsoft.IdentityModel.Tokens;     // کتابخانه JWT برای احراز هویت
using BCrypt.Net;                          // کتابخانه BCrypt برای هش کردن رمز عبور

namespace DeviceDataProcessor.Services
{
    /// <summary>
    /// پیاده‌سازی سرویس احراز هویت با JWT و BCrypt
    /// شامل متد‌های لاگین و ثبت‌نام کاربر
    /// </summary>
    public class AuthService : IAuthService
    {
        // مخزن کاربران (Repository) برای دسترسی به داده‌ها
        private readonly IUserRepository<User> _userRepository;

        // تنظیمات JWT برای ایجاد توکن
        private readonly JwtSettings _jwtSettings;

        /// <summary>
        /// سازنده کلاس AuthService
        /// </summary>
        /// <param name="userRepository">مخزن کاربران</param>
        /// <param name="jwtSettings">تنظیمات JWT</param>
        public AuthService(IUserRepository<User> userRepository, IOptions<JwtSettings> jwtSettings)
        {
            _userRepository = userRepository;
            _jwtSettings = jwtSettings.Value;
        }

        /// <summary>
        /// متد برای احراز هویت کاربر (ورود کاربر)
        /// </summary>
        /// <param name="username">نام کاربری</param>
        /// <param name="password">رمز عبور</param>
        /// <returns>توکن JWT یا null در صورت ناموفق بودن</returns>
        public async Task<string> AuthenticateAsync(string username, string password)
        {
            // جستجوی کاربر با نام کاربری وارد شده
            var user = await _userRepository.GetByUsernameAsync(username);

            // اگر کاربر وجود نداشته باشد یا رمز اشتباه باشد، null برمی‌گرداند
            if (user == null || !BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
                return null;

            // در صورت موفقیت، توکن JWT ایجاد می‌شود و بازمی‌گردد
            return GenerateJwtToken(user);
        }

        /// <summary>
        /// متد برای ثبت کاربر جدید
        /// </summary>
        /// <param name="username">نام کاربری</param>
        /// <param name="password">رمز عبور</param>
        /// <param name="role">نقش کاربر</param>
        /// <returns>true در صورت موفقیت - false در صورت وجود کاربر مشابه</returns>
        public async Task<bool> RegisterAsync(string username, string password, string role)
        {
            // بررسی اینکه نام کاربری خالی نباشد
            if (string.IsNullOrWhiteSpace(username))
                throw new ArgumentException("نام کاربری نمی‌تواند خالی باشد.");

            // بررسی طول رمز عبور (حداقل 6 کاراکتر)
            if (password.Length < 6)
                throw new ArgumentException("رمز عبور باید حداقل 6 کاراکتر باشد.");

            // بررسی اینکه آیا کاربر با این نام کاربری وجود دارد
            var existingUser = await _userRepository.GetByUsernameAsync(username);
            if (existingUser != null)
                return false; // کاربر قبلاً وجود دارد

            // هش کردن رمز عبور با استفاده از BCrypt
            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);

            // ایجاد کاربر جدید
            var user = new User
            {
                Username = username,
                PasswordHash = hashedPassword,
                Role = role
            };

            // ذخیره کاربر در دیتابیس
            await _userRepository.AddAsync(user);

            // بازگشت true به معنای موفقیت در ثبت کاربر
            return true;
        }

        /// <summary>
        /// تولید توکن JWT برای کاربر وارد شده
        /// </summary>
        /// <param name="user">کاربر وارد شده</param>
        /// <returns>رشته توکن JWT</returns>
        private string GenerateJwtToken(User user)
        {
            // ایجاد JwtSecurityTokenHandler برای مدیریت توکن
            var tokenHandler = new JwtSecurityTokenHandler();

            // تبدیل کلید مخفی به آرایه بایتی برای استفاده در توکن
            var key = Encoding.ASCII.GetBytes(_jwtSettings.Secret);

            // تنظیمات توکن: سوژه، اعتبار، امضای دیجیتال و ...
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                // اضافه کردن Claims به توکن
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()), // شناسه کاربر
                    new Claim(ClaimTypes.Name, user.Username),               // نام کاربری
                    new Claim(ClaimTypes.Role, user.Role)                   // نقش کاربر
                }),

                // زمان انقضای توکن (به دقیقه)
                Expires = DateTime.UtcNow.AddMinutes(_jwtSettings.TokenValidityInMinutes),

                // تنظیمات امضای توکن با الگوریتم HmacSha256
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature)
            };

            // ایجاد توکن با استفاده از تنظیمات
            var token = tokenHandler.CreateToken(tokenDescriptor);

            // تبدیل توکن به رشته و بازگرداندن آن
            return tokenHandler.WriteToken(token);
        }
    }
}