using DynamicNavigationMVC.data;
using DynamicNavigationMVC.Models;
using DynamicNavigationMVC.Repository;
using Microsoft.EntityFrameworkCore;

namespace DynamicNavigationMVC.Services.CategoryService
{
    public class CategoryService : IDataAccess<Category>
    {   
        private readonly NavDbContext _context;

        public CategoryService(NavDbContext context)
        {
            _context = context;
            
        }
        
        public async Task Add(Category entity)
        {
            try
            {
                await _context.category.AddAsync(entity);
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
                var category = await _context.category.FindAsync(id);
                if (category == null)
                {
                    throw new KeyNotFoundException("Category not found");
                }
                _context.category.Remove(category);
                await _context.SaveChangesAsync();
            }
            catch(Exception ex)
            {
                await Console.Out.WriteLineAsync(ex.Message);
            }
        }

        public async Task<IEnumerable<Category>> GetAll()
        {
            List<Category> categories = await _context.category
                .Include(c => c.ParentCategory)
                .Include(c => c.ChildCategory)
                .ToListAsync();
            await LoadChildCategories(categories);
            return categories;
        }

        public async Task<IEnumerable<Category>> GetAllTopLevelCategories()
        {
            List<Category> categories = await _context.category.Include(c => c.ParentCategory).Include(c => c.ChildCategory)
               .Where(c => c.ParentCategory.Count == 0).ToListAsync();
            await LoadChildCategories(categories);
            return categories;
        }

        public async Task<IEnumerable<Category>> GetLeafCategories() 
        {
            List<Category> categories = await _context.category.Include(c => c.ParentCategory).Include(c => c.ChildCategory)
               .Where(c => c.ChildCategory.Count == 0).ToListAsync();
            await LoadChildCategories(categories);
            return categories;
        }

        public async Task<Category> GetById(int id)
        {
            Category? category = await _context.category.Include(c => c.ParentCategory).Include(c => c.ChildCategory)
                .FirstOrDefaultAsync(c => c.Id == id);
 
            
            await LoadChildCategories(category!);
            return category!;
        }

        public async Task Update(Category entity)
        {
            try
            {
                var existingEntity = await _context.category.Include(c=>c.ParentCategory)
                    .FirstOrDefaultAsync(c=>c.Id == entity.Id);

                _context.Entry<Category>(existingEntity!).CurrentValues.SetValues(entity);

                existingEntity!.ParentCategory = entity.ParentCategory;

                await _context.SaveChangesAsync();
            }
            catch(Exception ex) 
            {
                await Console.Out.WriteLineAsync(ex.Message);
            }
        }

        private async Task LoadChildCategories(IEnumerable<Category> categories)
        {
            foreach (var category in categories)
            {
                // Load child categories recursively
                await _context.Entry(category)
                    .Collection(c => c.ChildCategory)
                    .LoadAsync();
                if (category.ChildCategory.Any())
                    // Recursively load child categories of child categories
                    await LoadChildCategories(category.ChildCategory);
            }
        }

        private async Task LoadChildCategories(Category category)
        {
            
            // Load child categories recursively
            await _context.Entry(category)
                .Collection(c => c.ChildCategory)
                .LoadAsync();
            if (category.ChildCategory!.Any())
                // Recursively load child categories of child categories
                await LoadChildCategories(category.ChildCategory!);
            
        }

        

    }
}
