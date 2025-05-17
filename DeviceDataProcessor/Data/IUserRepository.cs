using DeviceDataProcessor.Models;
using System.Threading.Tasks;

namespace DeviceDataProcessor.Data
{
    /// <summary>
    /// اینترفیس ریپازیتوری کاربران
    /// </summary>
    public interface IUserRepository : IRepository<User>
    {
        /// <summary>
        /// دریافت کاربر بر اساس نام کاربری به صورت غیرحساس به حروف
        /// </summary>
        /// <param name="username">نام کاربری</param>
        /// <returns>کاربر یافت شده یا null</returns>
        Task<User> GetByUsernameAsync(string username);
    }
}
