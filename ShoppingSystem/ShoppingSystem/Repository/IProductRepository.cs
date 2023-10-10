using ShoppingSystem.DTO;
using ShoppingSystem.Models;

namespace ShoppingSystem.Repository
{
    public interface IProductRepository : IRepository<Product>
    {
        Task<Categories> GetCategoryWithProducts(int categoryID);
        Task CreateAsync(ProductDTO product);
        Task UpdateAsync(int id ,ProductDTO product);
        Task<List<Product>> SearchByName(string name);
        Task SoftDeleteItemAsync(int itemId);
        Task SoftDeleteItemsAsync(IEnumerable<int> itemIds);
        Task<List<Product>> SearchByDate(DateTime date);

    }


}
