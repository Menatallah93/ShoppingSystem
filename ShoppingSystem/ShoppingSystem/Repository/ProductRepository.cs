using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using ShoppingSystem.DTO;
using ShoppingSystem.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ShoppingSystem.Repository
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        private readonly Context _context;
        public ProductRepository(Context context) : base(context)
        {
            _context = context;
        }

        public async Task CreateAsync(ProductDTO productdto)
        {
            var product = new Product();

            product.Name = productdto.Name;
            product.ProductPicture = productdto.ProductPicture;
            product.CategoriesID = productdto.CategoriesID;
            

            CreateAsync(product);
        }

        public async Task<List<Product>> SearchByDate(DateTime date)
        {
            var products = _context.Products
                .Where(p => p.AddedDate == date.Date)
                .ToList();
            return products;
        }

        public async Task<List<Product>> SearchByName(string name)
        {
            var products = _context.Products.Where(p => p.Name == name).ToList();

            return products;
        }

        public async Task UpdateAsync(int id, ProductDTO productdto)
        {
            var existingProduct = GetAllById(p => p.Id == id).FirstOrDefault();

            if (existingProduct != null)
            {
                existingProduct.Name = productdto.Name;
                existingProduct.ProductPicture = productdto.ProductPicture;
                existingProduct.CategoriesID = productdto.CategoriesID;

                await UpdateAsync(existingProduct);  
            }
            else
            {
                throw new ArgumentException($"Product with ID {id} not found.");
            }
        }


        public async Task SoftDeleteItemAsync(int itemId)
        {
            var item = await _context.Products.FirstOrDefaultAsync(c => c.Id == itemId);

            if (item == null)
            {
                return;
            }

            item.IsDeleted = true;
            await _context.SaveChangesAsync();
        }


        public async Task SoftDeleteItemsAsync(IEnumerable<int> itemIds)
        {
            var items = await _context.Products
                                      .Where(p => itemIds.Contains(p.Id))
                                      .ToListAsync();

            foreach (var item in items)
            {
                item.IsDeleted = true;
            }

            await _context.SaveChangesAsync();
        }

        public async Task<Categories> GetCategoryWithProducts(int categoryID)
        {
            var category = await _context.Categories.FindAsync(categoryID);

            if (category == null )
            {
                return null;
            }

            var products = await _context.Products
                .Where(o => !o.IsDeleted && o.CategoriesID == categoryID)
                .ToListAsync();

            return new Categories
            {
                Id= category.Id,
                Name = category.Name,
                product = products
            };
        }
    }
}
