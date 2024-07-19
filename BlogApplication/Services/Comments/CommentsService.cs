using BlogApplication.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;

namespace BlogApplication.Services.Comments
{
    public class CommentsService : ICommentsService
    {
        private readonly BlogDbContext _context;

        public CommentsService(BlogDbContext context)
        {
            _context = context;
        }



        public async Task CreateCommentAsync(Comment comment)
        {
            await _context.Comments.AddAsync(comment);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteCommentByIdAsync(int id)
        {

            var commentToDelete = await GetCommentByIdAsync(id);


        
            _context.Comments.Remove(commentToDelete);
            await _context.SaveChangesAsync();
        }

        public async Task EditCommentAsync(Comment comment)
        {
            _context.Comments.Update(comment);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> ExistsById(int id) => await _context.Comments.AnyAsync(a => a.CommentId == id);


        public async Task<Comment> GetCommentByIdAsync(int id) => _context.Comments.FirstOrDefault(a => a.CommentId == id);


        public async Task<IEnumerable<Comment>> GetCommentsAsync() => await _context.Comments.ToListAsync();

    }
}
