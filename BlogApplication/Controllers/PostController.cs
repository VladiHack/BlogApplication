using BlogApplication.Models;
using BlogApplication.Services.Categories;
using BlogApplication.Services.Comments;
using BlogApplication.Services.Posts;
using BlogApplication.Suppliers;
using BlogApplication.Validators;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace BlogApplication.Controllers
{
    public class PostController:Controller
    {

        private readonly BlogDbContext _context;
        private readonly IPostsService _postService;
        private readonly ICategoriesService _categoryService;
        private object value;
        private IPostsService @object;

        public PostController(BlogDbContext context, IPostsService postService, ICategoriesService categoryService)
        {
            _context = context;
            _postService = postService;
            _categoryService = categoryService;
        }

        public async Task<IActionResult> Index()
        {
            ViewBag.userId = UserIdSupplier.id;
            ViewBag.isAdmin = AdminRoleSupplier.isAdmin;

            return View(_context.Posts.Include(c=>c.Category).Include(a=>a.Author).Include(com=>com.Comments));
        }
        public async Task<IActionResult> CreateAsync()
        {
            var categories = await _categoryService.GetCategoriesAsync();
            ViewBag.Categories = categories;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Post post)
        {
            string msg = "";

         
            List<Post> posts = (List<Post>)await _postService.GetPostsAsync();
            if (posts.Count == 0)
            {
                post.PostId = 1;
            }
            else
            {
                post.PostId = posts[posts.Count() - 1].PostId + 1;
            }
            
            // Set the AuthorId to the current user's ID
            post.AuthorId = UserIdSupplier.id;

            msg = PostValidator.ReturnErrorsCreate(posts, post);
            ViewBag.Message = msg;

            if (msg == "")
            {
                await _postService.CreatePostAsync(post);
                return RedirectToAction(nameof(Index));
            }
            return View(post);
        }

        public async Task<IActionResult> Edit(int id)
        {

        

            var post = _context.Posts
                .Include(p => p.Category)
                .AsNoTracking()
                .FirstOrDefault(y => y.PostId == id);

            var categories = await _categoryService.GetCategoriesAsync();
            ViewBag.Categories = categories;

            return View(post);
        }
        public IActionResult Delete(int id)
        {
            var post = _context.Posts.Include(p=>p.Category).FirstOrDefault(x => x.PostId == id);
            return View(post);

        }
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (await _postService.ExistsById(id))
            {
                var post = await _postService.GetPostByIdAsync(id);
                await _postService.DeletePostByIdAsync(id);
            }
  

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Post post)
        {
            // Retrieve the existing Post entity from the database
      
            var existingPost = await _postService.GetPostByIdAsync(id);

            // Update the properties of the existing Post entity
            existingPost.Title = post.Title;
            existingPost.Content = post.Content;
            existingPost.CategoryId = post.CategoryId;

            // Attach the updated entity to the DbContext and mark it as Modified
            _context.Attach(existingPost).State = EntityState.Modified;

            string msg = PostValidator.ReturnErrorsEdit(_context.Posts.ToList(), existingPost, id);
            ViewBag.Message = msg;

            if (msg == "")
            {
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "Post");
            }

            var categories = await _categoryService.GetCategoriesAsync();
            ViewBag.Categories = categories;

            return View(existingPost);
        }

        public IActionResult Details(int id)
        {
            var posts = _context.Posts.Include(p=>p.Category).Include(a=>a.Author).FirstOrDefault(y => y.PostId == id);
            return View(posts);
        }
    }
}
