using Exeed.DAL.Repositories;
using Exeed.Domain.Models;

namespace Exeed.Managers
{
    public class DesireManager
    {
        private readonly IDesireRepository _repository;

        public DesireManager(IDesireRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<Desire>> GetAsync()
        {
            var desires = await _repository.ReadAsync();
            return desires.ToList();
        }

        public async Task<Desire?> GetAsync(string desireId)
        {
            return await _repository.ReadAsync(desireId);
        }

        public async Task<Desire?> NewWish(string text)
        {
            Desire desire = new Desire()
            {
                Text = text
            };

            bool created = await _repository.CreateAsync(desire);
            return created ? desire : null;
        }

        public async Task UploadPhoto(Desire desire, string photoPath)
        {
            desire.PhotoPath = photoPath;
            await _repository.UpdateAsync(desire);
        }

        public async Task VerifyAsync(string desireId, bool isVerified)
        {
            var desire = await _repository.ReadAsync(desireId);
            if (desire.IsVerified.HasValue) return;

            desire.IsVerified = isVerified;
            await _repository.UpdateAsync(desire);
        }

        public async Task<bool> WinAsync(Desire desire)
        {
            desire.IsWon = true;
            return await _repository.UpdateAsync(desire);
        }
    }
}
