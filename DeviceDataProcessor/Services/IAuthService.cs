using DeviceDataProcessor.Models;
using System.Threading.Tasks;

namespace DeviceDataProcessor.Services
{
    // اینترفیس برای احراز هویت کاربران
    public interface IAuthService
    {
        Task<string> AuthenticateAsync(string username, string password); // متد برای احراز هویت کاربر
        Task<bool> RegisterAsync(string username, string password, UserRole role); // متد برای ثبت نام کاربر
    }
}