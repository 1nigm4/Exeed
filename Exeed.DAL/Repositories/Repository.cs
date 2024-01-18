using Exeed.Domain;
using Exeed.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Exeed.DAL.Repositories
{
    public class Repository<TModel> : IRepository<TModel> where TModel : class, IModel
    {
        protected readonly ApplicationDbContext _context;

        public Repository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> CreateAsync(TModel model)
        {
            model.Id = Guid.NewGuid().ToString();
            var entry = await _context.Set<TModel>()
                .AddAsync(model);

            if (entry.State != EntityState.Added) return false;

            return await SaveChangesAsync(entry);
        }

        public async Task<IEnumerable<TModel>> ReadAsync()
        {
            return await _context.Set<TModel>()
                .ToListAsync();
        }

        public async Task<TModel?> ReadAsync(string id)
        {
            return await _context.Set<TModel>()
                .FindAsync(id);
        }

        public async Task<bool> UpdateAsync(TModel model)
        {
            var entry = _context.Set<TModel>()
                .Update(model);

            if (entry?.State != EntityState.Modified) return false;

            return await SaveChangesAsync(entry);
        }

        public async Task<bool> DeleteAsync(TModel model)
        {
            var entry = _context.Set<TModel>()
                .Remove(model);

            if (entry?.State != EntityState.Deleted) return false;

            return await SaveChangesAsync(entry);
        }

        private async Task<bool> SaveChangesAsync(EntityEntry entry)
        {
            try
            {
                await _context.SaveChangesAsync();
                await TryUpdateEntryAsync(entry);
                return true;
            }
            catch
            {
                return false;
            }
        }

        protected async Task<bool> TryUpdateEntryAsync(EntityEntry entry)
        {
            try
            {
                foreach (var navigation in entry.Navigations)
                    if (!navigation.IsLoaded)
                        await navigation.LoadAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
