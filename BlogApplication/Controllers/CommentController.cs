using BlogApplication.Models;
using BlogApplication.Services.Comments;
using BlogApplication.Services.Posts;
using BlogApplication.Suppliers;
using BlogApplication.Validators;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BlogApplication.Controllers
{
    public class CommentController : Controller
    {

        private readonly BlogDbContext _context;
        private readonly ICommentsService _commentsService;
        private readonly IPostsService _postsService;

        public CommentController(BlogDbContext context, ICommentsService commentService,IPostsService postService)
        {
            _context = context;
            _commentsService = commentService;
            _postsService = postService;
        }

        public async Task<IActionResult> Index(int? postId)
        {
            ViewBag.userId = UserIdSupplier.id;
            ViewBag.isAdmin = AdminRoleSupplier.isAdmin;

            ViewBag.PostId = postId; // Store the postId in the ViewBag

            IQueryable<Comment> comments = _context.Comments
                .Include(c => c.Author)
                .Include(c => c.Post);

            if (postId.HasValue)
            {
                comments = comments.Where(c => c.PostId == postId.Value);
            }

            return View(comments.ToList());
        }

        public async Task<IActionResult> Create(int postId)
        {
        

            ViewBag.PostId = postId;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int postId, Comment comment)
        {
            // Set the PostId from the parameter
            comment.PostId = postId;

            List<Comment> comments = (List<Comment>)await _commentsService.GetCommentsAsync();
            if(comments.Count==0)
            {
                comment.CommentId = 1;
            }
            else
            {
                comment.CommentId = comments[comments.Count - 1].CommentId + 1;
            }

            // Set the AuthorId to the current user's ID
            comment.AuthorId = UserIdSupplier.id;

            // Get the Bulgarian time zone
            TimeZoneInfo bulgarianTimeZone = TimeZoneInfo.FindSystemTimeZoneById("FLE Standard Time");

            // Convert the current UTC time to Bulgarian time
            comment.CreatedAt = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, bulgarianTimeZone);

            string msg = CommentValidator.ReturnErrorsCreate((List<Comment>)await _commentsService.GetCommentsAsync(), comment);
            ViewBag.Message = msg;

            if (msg == "")
            {
                await _commentsService.CreateCommentAsync(comment);
                return RedirectToAction(nameof(Index), new { postId = comment.PostId });
            }

            // If there are errors, return the view with the model
            return View(comment);
        }

        public async Task<IActionResult> Edit(int id)
        {
      

            var comment = _context.Comments
                .Include(p => p.Post)
                .AsNoTracking()
                .FirstOrDefault(y => y.CommentId == id);

            var posts = await _postsService.GetPostsAsync();
            ViewBag.Posts = posts;
            ViewBag.PostId = comment.PostId;
            return View(comment);
        }
        public async Task<IActionResult> Delete(int id)
        {


            var comment = _context.Comments.Include(p => p.Post).Include(p=>p.Author).FirstOrDefault(x => x.CommentId == id);
            ViewBag.PostId = comment.PostId;

            return View(comment);

        }
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var comment = await _commentsService.GetCommentByIdAsync(id);
            if (comment != null)
            {
                //Remove all connected things
                await _commentsService.DeleteCommentByIdAsync(id);
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Comment comment)
        {
            // Check if the comment exists
            var existingComment = await _context.Comments.FindAsync(id);
   

            // Update the properties you want to allow editing
            existingComment.Content = comment.Content;

            string msg = CommentValidator.ReturnErrorsEdit(_context.Comments.ToList(), existingComment, id);
            ViewBag.Message = msg;

            if (msg == "")
            {
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index), new { postId = existingComment.PostId });
            }

            // If there are errors, return the view with the model
            return View(existingComment);
        }

        public async Task<IActionResult> Details(int id)
        {
 

            var comment = _context.Comments.Include(p => p.Post).Include(a => a.Author).FirstOrDefault(y => y.CommentId == id);
            ViewBag.PostId = comment.PostId;

            return View(comment);
        }
    }
}
