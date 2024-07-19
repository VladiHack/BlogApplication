using BlogApplication.Models;
using Microsoft.EntityFrameworkCore;

namespace BlogApplication.Services.Users
{
    public class UserService : IUserService
    {
        private readonly BlogDbContext _context;

        public UserService(BlogDbContext context)
        {
            _context = context;
        }


        public async Task CreateUserAsync(User user)
        {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteUserByIdAsync(int id)
        {
            var userToDelete = await GetUserByIdAsync(id);

            // Retrieve all posts by the user
            var posts = _context.Posts.Where(p => p.AuthorId == id).ToList();

            // Remove all comments associated with those posts
            var commentsToRemove = posts.SelectMany(post => _context.Comments.Where(c => c.PostId == post.PostId)).ToList();
            _context.Comments.RemoveRange(commentsToRemove);

            // Remove all posts
            _context.Posts.RemoveRange(posts);

            // Finally, remove the user
            _context.Users.Remove(userToDelete);

            // Perform a single SaveChangesAsync call outside the loops
            await _context.SaveChangesAsync();
        }

        public async Task EditUserAsync(User user)
        {

            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> ExistsById(int id) => await _context.Users.AnyAsync(a => a.UserId == id);


        public async Task<User> GetUserByIdAsync(int id) => _context.Users.FirstOrDefault(a => a.UserId == id);


        public async Task<IEnumerable<User>> GetUsersAsync() => await _context.Users.ToListAsync();       


    }
}
