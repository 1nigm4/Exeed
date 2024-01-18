using Exeed.Domain;
using Exeed.Domain.Models;

namespace Exeed.DAL.Repositories
{
    public class WinnerRepository : Repository<Winner>, IWinnerRepository
    {
        public WinnerRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
