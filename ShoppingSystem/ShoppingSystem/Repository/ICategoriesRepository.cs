using ShoppingSystem.DTO;
using ShoppingSystem.Models;

namespace ShoppingSystem.Repository
{
    public interface ICategoriesRepository : IRepository<Categories>
    {
        Task CreateAsync(CategoriesDTO categoriesDTO);
        Task<CategoriesDTO> UpdateAsync(int id, CategoriesDTO categoriesDTO);

    }
}
