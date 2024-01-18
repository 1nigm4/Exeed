using Exeed.Domain.Models;
using Exeed.Models;
using Microsoft.Extensions.Options;

namespace Exeed.Managers
{
    public class EventManager
    {
        private readonly IOptions<Configuration> _configuration;
        private readonly DesireManager _desireManager;
        private readonly WinnerManager _winnerManager;

        public EventManager(
            DesireManager desireManager,
            WinnerManager winnerManager,
            IOptions<Configuration> configuration)
        {
            _desireManager = desireManager;
            _winnerManager = winnerManager;
            _configuration = configuration;
        }

        public async Task CheckWinnersAsync()
        {
            DateTime now = DateTime.Now;
            DateTime? lastWinerDate = await _winnerManager.GetLastDateTimeWinnerAsync();
            var config = _configuration.Value;
            TimeSpan checkingRange = new TimeSpan(config.EventTime.Hours + 1, config.EventTime.Minutes, config.EventTime.Seconds);
            if ((lastWinerDate == null || (now.Day >= lastWinerDate?.Day + 7)) && (now.TimeOfDay > config.EventTime && now.TimeOfDay < checkingRange))
            {
                var winners = await GetWinnersAsync();
                await SetWinnersAsync(winners);
            }

            await Task.Delay(TimeSpan.FromHours(1));
        }

        public async Task<IEnumerable<Desire>> GetWinnersAsync()
        {
            var desires = await _desireManager.GetAsync();

            return desires.Where(desire => !desire.IsWon && (desire.IsVerified ?? false))
                .OrderByDescending(desire => desire.Likes.Count)
                .Take(3);
        }

        public async Task<bool> SetWinnersAsync(IEnumerable<Desire> desires)
        {
            if (desires.Count() != 3) return false;

            int prize = 1;
            foreach (Desire desire in desires)
            {
                Winner winner = new Winner()
                {
                    Desire = desire,
                    WonAt = DateTime.Now,
                    Prize = (Prize)prize++
                };

                bool isAdded = await _winnerManager.AddAsync(winner);
                if (!isAdded) return false;

                await _desireManager.WinAsync(desire);
            }

            return true;
        }
    }
}
