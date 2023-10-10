using System.Linq.Expressions;

namespace ShoppingSystem.Repository
{

    public interface IRepository<T> where T : class
    {
        List<T> GetAllById(Func<T, bool> predicate);
        Task<List<T>> GetAllAsync();
        void CreateAsync(T entity);
        Task UpdateAsync(T entity);
    }
}
