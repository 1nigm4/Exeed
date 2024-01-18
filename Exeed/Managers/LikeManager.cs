using Exeed.DAL.Repositories;
using Exeed.Domain.Models;

namespace Exeed.Managers
{
    public class LikeManager
    {
        private readonly ILikeRepository _likeRepository;

        public LikeManager(ILikeRepository likeRepository)
        {
            _likeRepository = likeRepository;
        }

        public async Task<bool> LikeAsync(Account account, Desire desire)
        {
            if (account.Likes.Count >= 3) return false;
            if (account.Likes.Any(like => like.Desire == desire)) return false;

            Like like = new Like()
            {
                Account = account,
                Desire = desire
            };

            return await _likeRepository.CreateAsync(like);
        }

        public async Task<bool> UnLikeAsync(Account account, Desire desire)
        {
            if (!account.Likes.Any(like => like.Desire == desire)) return false;

            Like like = account.Likes.First(like => like.Desire == desire);
            return await _likeRepository.DeleteAsync(like);
        }
    }
}
