using DeviceDataProcessor.Models;
using DeviceDataProcessor.Data;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using DeviceDataProcessor.Settings;

namespace DeviceDataProcessor.Services
{
    // پیاده‌سازی IAuthService برای مدیریت احراز هویت
    public class AuthService : IAuthService
    {
        private readonly IRepository<User> _userRepository; // مخزن کاربران
        private readonly JwtSettings _jwtSettings; // تنظیمات JWT

        public AuthService(IRepository<User> userRepository, IOptions<JwtSettings> jwtSettings)
        {
            _userRepository = userRepository; // دریافت مخزن کاربران
            _jwtSettings = jwtSettings.Value; // دریافت تنظیمات JWT
        }

        // متد برای احراز هویت کاربر
        public async Task<string> AuthenticateAsync(string username, string password)
        {
            var users = await _userRepository.GetAllAsync(); // دریافت همه کاربران
            var validUser = users.FirstOrDefault(u => u.Username == username && u.PasswordHash == password); // بررسی اعتبار کاربر

            if (validUser == null)
                return null; // در صورت نامعتبر بودن کاربر، null برمی‌گرداند

            var tokenHandler = new JwtSecurityTokenHandler(); // ایجاد توکن JWT
            var key = Encoding.ASCII.GetBytes(_jwtSettings.Secret); // کلید مخفی
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Name, validUser.Username), // اضافه کردن نام کاربری به توکن
                    new Claim(ClaimTypes.Role, validUser.Role) // اضافه کردن نقش کاربر به توکن
                }),
                Expires = DateTime.UtcNow.AddHours(_jwtSettings.ExpiryInHours), // زمان انقضا
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature) // امضای توکن
            };

            var token = tokenHandler.CreateToken(tokenDescriptor); // ایجاد توکن
            return tokenHandler.WriteToken(token); // بازگشت توکن
        }

        // متد برای ثبت نام کاربر جدید
        public async Task<bool> RegisterAsync(string username, string password, string role)
        {
            var user = new User { Username = username, PasswordHash = password, Role = role }; // ایجاد کاربر جدید
            await _userRepository.AddAsync(user); // افزودن کاربر به مخزن
            return true; // بازگشت موفقیت
        }
    }
}