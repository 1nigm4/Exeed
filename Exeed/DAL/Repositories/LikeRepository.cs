using Exeed.Data;
using Exeed.Data.Models;

namespace Exeed.DAL.Repositories
{
    public class LikeRepository : Repository<Like>, ILikeRepository
    {
        public LikeRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
