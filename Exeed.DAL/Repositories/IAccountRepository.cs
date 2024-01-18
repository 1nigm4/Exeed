using Exeed.Domain.Models;

namespace Exeed.DAL.Repositories
{
    public interface IAccountRepository : IRepository<Account>
    {
        Task<Account?> GetAsync(string userId);
        Task<Account?> GetAsync(Desire desire);
    }
}
