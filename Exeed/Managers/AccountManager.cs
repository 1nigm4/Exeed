using Exeed.DAL.Repositories;
using Exeed.Data.Models;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace Exeed.Managers
{
    public class AccountManager
    {
        private readonly IAccountRepository _repository;

        public AccountManager(IAccountRepository repository)
        {
            _repository = repository;
        }

        public async Task<bool> RegisterAsync(IdentityUser user, string firstName)
        {
            Account account = new Account
            {
                User = user,
                FirstName = firstName
            };
            bool result = await _repository.CreateAsync(account);
            return result;
        }

        public async Task<Account?> GetAsync(ClaimsPrincipal user)
        {
            if (user?.Identity?.Name == null) return null;
            return await _repository.GetAsync(user.Identity.Name);
        }

        public async Task<Account?> GetAsync(Desire desire)
        {
            return await _repository.GetAsync(desire);
        }

        public async Task<Account?> GetAsync(string accountId)
        {
            return await _repository.ReadAsync(accountId);
        }

        public async Task AddDesire(Account account, Desire desire)
        {
            if (account == null || desire == null || account.Desire != null) return;

            account.Desire = desire;
            await _repository.UpdateAsync(account);
        }

        /*public async Task<bool> AddWish(string accountId, Wish wish)
        {
            Account? account = await GetAsync(accountId);
            if (account == null) return false;

            account.Wish = wish;
            bool result = await _repository.UpdateAsync(account);
            return result;
        }

        public async Task<bool> LikeAsync(Account account, Wish wish)
        {
            if (wish == account.Wish) return false;
            account.LikedWishes.Add(wish);
            return await _repository.UpdateAsync(account);
        }*/
    }
}
