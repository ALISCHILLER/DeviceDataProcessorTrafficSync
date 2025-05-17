using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

using DeviceDataProcessor.Models;
using DeviceDataProcessor.Data;
using DeviceDataProcessor.Settings;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using BCrypt.Net;

namespace DeviceDataProcessor.Services
{
    /// <summary>
    /// سرویس احراز هویت کاربران با JWT و BCrypt
    /// شامل متدهای ورود و ثبت نام
    /// </summary>
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly JwtSettings _jwtSettings;

        public AuthService(IUserRepository userRepository, IOptions<JwtSettings> jwtSettings)
        {
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            _jwtSettings = jwtSettings?.Value ?? throw new ArgumentNullException(nameof(jwtSettings));
        }

        /// <summary>
        /// احراز هویت کاربر و تولید توکن JWT
        /// </summary>
        public async Task<string> AuthenticateAsync(string username, string password)
        {
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
                return null;

            var normalizedUsername = username.Trim().ToLower();
            var user = await _userRepository.GetByUsernameAsync(normalizedUsername);

            if (user == null || !BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
                return null;

            return GenerateJwtToken(user);
        }

        /// <summary>
        /// ثبت نام کاربر جدید
        /// </summary>
        public async Task<bool> RegisterAsync(string username, string password, string role)
        {
            username = username?.Trim();
            password = password?.Trim();

            if (string.IsNullOrWhiteSpace(username))
                throw new ArgumentException("نام کاربری نمی‌تواند خالی باشد.");

            if (string.IsNullOrWhiteSpace(password) || password.Length < 6)
                throw new ArgumentException("رمز عبور باید حداقل ۶ کاراکتر باشد.");

            var existingUser = await _userRepository.GetByUsernameAsync(username.ToLower());
            if (existingUser != null)
                return false;

            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);

            if (!Enum.TryParse<UserRole>(role, out var parsedRole))
                throw new ArgumentException("نقش کاربر نامعتبر است.");

            var user = new User
            {
                Username = username,
                PasswordHash = hashedPassword,
                Role = parsedRole
            };

            await _userRepository.AddAsync(user);
            return true;
        }

        /// <summary>
        /// تولید توکن JWT برای کاربر
        /// </summary>
        private string GenerateJwtToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtSettings.Secret);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Role, user.Role.ToString())
            };

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(_jwtSettings.TokenValidityInMinutes),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
