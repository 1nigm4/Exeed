using Exeed.DAL.Repositories;
using Exeed.Domain.Models;
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
    }
}
