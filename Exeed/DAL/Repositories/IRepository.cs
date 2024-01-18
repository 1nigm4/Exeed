using Exeed.Data.Models;

namespace Exeed.DAL.Repositories
{
    public interface IRepository<TModel> where TModel : class, IModel
    {
        public Task<bool> CreateAsync(TModel model);
        public Task<IEnumerable<TModel>> ReadAsync();
        public Task<TModel?> ReadAsync(string id);
        public Task<bool> UpdateAsync(TModel model);
        public Task<bool> DeleteAsync(TModel model);
    }
}
