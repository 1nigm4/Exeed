using Exeed.Domain;
using Exeed.Domain.Models;

namespace Exeed.DAL.Repositories
{
    public class DesireRepository : Repository<Desire>, IDesireRepository
    {
        public DesireRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
