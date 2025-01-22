using System.Threading.Tasks;

namespace DeviceDataProcessor.Services
{
    public interface IAuthService
    {
        Task<string> AuthenticateAsync(string username, string password); // متد برای احراز هویت کاربر
    }
}