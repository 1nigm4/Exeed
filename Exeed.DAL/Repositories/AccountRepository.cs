using Exeed.Domain;
using Exeed.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Exeed.DAL.Repositories
{
    public class AccountRepository : Repository<Account>, IAccountRepository
    {
        public AccountRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<Account?> GetAsync(string userName)
        {
            return await _context.Set<Account>()
                .FirstOrDefaultAsync(account => account.User.UserName == userName);
        }

        public async Task<Account?> GetAsync(Desire desire)
        {
            if (desire == null) return null;

            return await _context.Set<Account>()
                .FirstOrDefaultAsync(account => account.Desire.Id == desire.Id);
        }
    }
}
