using DeviceDataProcessor.Models;
using DeviceDataProcessor.Data;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using DeviceDataProcessor.Settings;

namespace DeviceDataProcessor.Services
{
    public class AuthService : IAuthService
    {
        private readonly IRepository<User> _userRepository; // مخزن کاربران
        private readonly JwtSettings _jwtSettings; // تنظیمات JWT

        public AuthService(IRepository<User> userRepository, JwtSettings jwtSettings)
        {
            _userRepository = userRepository; // دریافت مخزن کاربران
            _jwtSettings = jwtSettings; // دریافت تنظیمات JWT
        }

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
                    new Claim(ClaimTypes.Name, validUser.Username),
                    new Claim(ClaimTypes.Role, validUser.Role)
                }),
                Expires = DateTime.UtcNow.AddHours(1), // زمان انقضا
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature) // امضای توکن
            };

            var token = tokenHandler.CreateToken(tokenDescriptor); // ایجاد توکن
            return tokenHandler.WriteToken(token); // بازگشت توکن
        }
    }
}