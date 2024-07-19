using BlogApplication.Models;
using BlogApplication.Services.Categories;
using BlogApplication.Services.Comments;
using BlogApplication.Suppliers;
using BlogApplication.Validators;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BlogApplication.Controllers
{
    public class CategoryController:Controller
    {
        private readonly BlogDbContext _context;
        private readonly ICategoriesService _categoriesService;

        public CategoryController(BlogDbContext context, ICategoriesService categoriesService)
        {
            _context = context;
            _categoriesService = categoriesService;
        }

        public async Task<IActionResult> Index()
        {
            ViewBag.userId = UserIdSupplier.id;
            ViewBag.isAdmin = AdminRoleSupplier.isAdmin;

            var categories = await _categoriesService.GetCategoriesAsync();
            return View(categories);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Category category)
        {
            string msg = "";

            List<Category> categories = (List<Category>)await _categoriesService.GetCategoriesAsync();
            if (categories.Count == 0)
            {
                category.CategoryId = 1;
            }
            else
            {
                //Get the id of the last element and add 1
                categories = categories.OrderBy(c=>c.CategoryId).ToList();

                category.CategoryId = categories[categories.Count() - 1].CategoryId + 1;
            }
            msg = CategoryValidator.ReturnErrorsCreate(categories, category);
            ViewBag.Message = msg;

            if (msg == "")
            {
                await _categoriesService.CreateCategoryAsync(category);
                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var category = await _categoriesService.GetCategoryByIdAsync(id);
            return View(category);

        }
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var category = await _categoriesService.GetCategoryByIdAsync(id);
            if (category != null)
            {
                //Remove all connected things
               await _categoriesService.DeleteCategoryByIdAsync(id);
            }

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Details(int id)
        {
            var category =await _categoriesService.GetCategoryByIdAsync(id);
            return View(category);
        }
    }
}
