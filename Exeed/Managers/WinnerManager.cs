using Exeed.DAL.Repositories;
using Exeed.Domain.Models;

namespace Exeed.Managers
{
    public class WinnerManager
    {
        private readonly IWinnerRepository _repository;

        public WinnerManager(IWinnerRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<Winner>> GetWinnersAsync()
        {
            var winners = await _repository.ReadAsync();
            winners.OrderBy(w => w.WonAt);

            return winners.OrderByDescending(w => w.WonAt);
        }

        public async Task<bool> AddAsync(Winner winner)
        {
            return await _repository.CreateAsync(winner);
        }

        public async Task<DateTime?> GetLastDateTimeWinnerAsync()
        {
            var winners = await _repository.ReadAsync();
            return winners.OrderByDescending(winner => winner.WonAt).FirstOrDefault()?.WonAt;
        }
    }
}
