using Microsoft.EntityFrameworkCore;
using ShoppingSystem.Models;
using System.Linq.Expressions;

namespace ShoppingSystem.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly Context _context;

        public Repository(Context context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public void CreateAsync(T entity)
        {
            _context.Set<T>().Add(entity);
            _context.SaveChanges();
        }

        public async Task<List<T>> GetAllAsync()
        {
            return _context.Set<T>().ToList();
        }

        public List<T> GetAllById(Func<T, bool> predicate)
        {
            return _context.Set<T>().Where(predicate).ToList();
        }

        public async Task UpdateAsync(T Work)
        {
            _context.Set<T>().Update(Work);
            await _context.SaveChangesAsync();
        }
    }
}
