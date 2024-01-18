using Exeed.Data;
using Exeed.Data.Models;

namespace Exeed.DAL.Repositories
{
    public class WinnerRepository : Repository<Winner>, IWinnerRepository
    {
        public WinnerRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
