using BlogApplication.Models;
using Microsoft.EntityFrameworkCore;

namespace BlogApplication.Services.Posts
{
    public class PostService : IPostsService
    {
        private readonly BlogDbContext _context;

        public PostService(BlogDbContext context)
        {
            _context = context;
        }

        public async Task CreatePostAsync(Post post)
        {
            await _context.Posts.AddAsync(post);
            await _context.SaveChangesAsync();
        }

        public async Task DeletePostByIdAsync(int id)
        {
            var postToDelete = await GetPostByIdAsync(id);

         
                List<Comment> comments = _context.Comments.Where(c => c.PostId == postToDelete.PostId).ToList();
                foreach (Comment comment in comments)
                {
                    _context.Comments.Remove(comment);
                    await _context.SaveChangesAsync();
                }
                _context.Posts.Remove(postToDelete);
                await _context.SaveChangesAsync();
           
        }

        public async Task EditPostAsync(Post post)
        {
            _context.Posts.Update(post);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> ExistsById(int id) => await _context.Posts.AnyAsync(a => a.PostId == id);


        public async Task<Post> GetPostByIdAsync(int id) => _context.Posts.FirstOrDefault(a => a.PostId == id);


        public async Task<IEnumerable<Post>> GetPostsAsync() => await _context.Posts.ToListAsync();

    }
}
