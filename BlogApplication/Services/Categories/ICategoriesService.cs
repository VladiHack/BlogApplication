using BlogApplication.Models;

namespace BlogApplication.Services.Categories
{
    public interface ICategoriesService
    {
        Task<IEnumerable<Category>> GetCategoriesAsync();
        Task<bool> ExistsById(int id);
        Task<Category> GetCategoryByIdAsync(int id);
        Task CreateCategoryAsync(Category category);
        Task DeleteCategoryByIdAsync(int id);
    }
}
