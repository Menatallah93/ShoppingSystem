using ShoppingSystem.DTO;
using ShoppingSystem.Models;

namespace ShoppingSystem.Repository
{
    public class CategoriesRepository : Repository<Categories>, ICategoriesRepository
    {
        public CategoriesRepository(Context context) : base(context)
        {
        }

        public async Task CreateAsync(CategoriesDTO categoriesDTO)
        {
            var product = new Categories
            {
                Name = categoriesDTO.Name,
            };

            CreateAsync(product);
        }

        public async Task<CategoriesDTO> UpdateAsync(int id, CategoriesDTO categoriesDTO)
        {
            var existingProduct = GetAllById(p => p.Id == id).FirstOrDefault();

            if (existingProduct != null)
            {
                existingProduct.Name = categoriesDTO.Name;

                await UpdateAsync(existingProduct);
                return categoriesDTO;
            }
            else
            {
                throw new ArgumentException($"Product with ID {id} not found.");
            }
        }
    }
}
