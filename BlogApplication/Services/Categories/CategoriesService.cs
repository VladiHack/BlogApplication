using BlogApplication.Models;
using Microsoft.EntityFrameworkCore;

namespace BlogApplication.Services.Categories
{
    public class CategoriesService : ICategoriesService
    {

        private readonly BlogDbContext _context;

        public CategoriesService(BlogDbContext context)
        {
            _context = context;
        }


        public async Task CreateCategoryAsync(Category category)
        {
            await _context.Categories.AddAsync(category);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteCategoryByIdAsync(int id)
        {
            var categoryToDelete = await GetCategoryByIdAsync(id);



            _context.Categories.Remove(categoryToDelete);
            await _context.SaveChangesAsync();
        }

 

        public async Task<bool> ExistsById(int id) => await _context.Categories.AnyAsync(a => a.CategoryId == id);


        public async Task<IEnumerable<Category>> GetCategoriesAsync() => await _context.Categories.ToListAsync();


        public async Task<Category> GetCategoryByIdAsync(int id) => _context.Categories.FirstOrDefault(a => a.CategoryId == id);

    }
}
