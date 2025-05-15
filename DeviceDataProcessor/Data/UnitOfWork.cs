using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace DeviceDataProcessor.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;

            // --- Initialize Repositories ---
            Users = new UserRepository(context);
         //   Devices = new DeviceRepository(context);
            DeviceData = new DeviceDataRepository(context);
        }

        // --- Properties ---
        public IUserRepository Users { get; }
       // public IDeviceRepository Devices { get; }
        public IDeviceDataRepository DeviceData { get; }

        // --- Save Changes ---
        public async Task<int> SaveChangesAsync()
            => await _context.SaveChangesAsync();
    }
}