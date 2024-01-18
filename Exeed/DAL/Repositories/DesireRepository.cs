using Exeed.Data;
using Exeed.Data.Models;

namespace Exeed.DAL.Repositories
{
    public class DesireRepository : Repository<Desire>, IDesireRepository
    {
        public DesireRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
