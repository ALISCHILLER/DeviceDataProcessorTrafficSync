using DeviceDataProcessor.Models;

namespace DeviceDataProcessor.Data
{
    public interface IUserRepository : IRepository<User>
    {
        Task<User> GetByUsernameAsync(string username); // دریافت موجودیت بر اساس نام کاربری
    }
}
