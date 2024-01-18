using Exeed.Domain;
using Exeed.Domain.Models;

namespace Exeed.DAL.Repositories
{
    public class LikeRepository : Repository<Like>, ILikeRepository
    {
        public LikeRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
