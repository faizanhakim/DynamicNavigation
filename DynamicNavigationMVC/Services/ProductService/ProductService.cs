using DynamicNavigationMVC.data;
using DynamicNavigationMVC.Models;
using DynamicNavigationMVC.Repository;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;

namespace DynamicNavigationMVC.Services.ProductService
{
    public class ProductService : IDataAccess<Product>
    {
        private readonly NavDbContext _context;
        public ProductService(NavDbContext context)
        {
            _context = context;
        }
        public async Task Add(Product entity)
        {
            try
            {
                await _context.products.AddAsync(entity);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync(ex.Message);
            }
        }
        public async Task Delete(int id)
        {
            try
            {
                var product = await _context.products.FindAsync(id);
                if (product == null)
                {
                    throw new KeyNotFoundException("Product not found");
                }
                _context.products.Remove(product);
                await _context.SaveChangesAsync();
            }
            catch(Exception ex) 
            {
                await Console.Out.WriteLineAsync(ex.Message);
            }
        }
        public async Task<IEnumerable<Product>> GetAll()
        {
            var products = await _context.products.ToListAsync();
            return products;
        }
        public async Task<Product> GetById(int id)
        {
            var product = await _context.products.FirstOrDefaultAsync(p => p.Id == id);
            return product!;
        }
        public async Task<IEnumerable<Product>> GetByCategory(int categoryId)
        {

			var categoryIds = GetChildCategoryIds(categoryId);

			// Retrieve products belonging to the specified category or its children categories
			var products = await _context.products
				.Where(p => p.Category.Any(c => categoryIds.Contains(c.Id)))
				.ToListAsync();

			return products;
		}
		private List<int> GetChildCategoryIds(int categoryId)
		{
			var allCategoryIds = new List<int> { categoryId };
			var childCategoryIds = _context.category
				.Where(c => c.ParentCategory.Any(pc => pc.Id == categoryId))
				.Select(c => c.Id)
				.ToList();

			foreach (var childId in childCategoryIds)
			{
				allCategoryIds.AddRange(GetChildCategoryIds(childId));
			}

			return allCategoryIds;
		}
		public async Task Update(Product entity)
        {
            try
            {
                var existingEntity = await _context.products.Include(p => p.Category)
                    .FirstOrDefaultAsync(p => p.Id == entity.Id);

                _context.Entry<Product>(existingEntity!).CurrentValues.SetValues(entity);

                existingEntity!.Category = entity.Category;


                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync(ex.Message);
            }
        }
    }
}
