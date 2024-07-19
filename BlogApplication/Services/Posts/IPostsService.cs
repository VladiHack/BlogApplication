using BlogApplication.Models;

namespace BlogApplication.Services.Posts
{
    public interface IPostsService
    {
        Task<IEnumerable<Post>> GetPostsAsync();
        Task<bool> ExistsById(int id);
        Task<Post> GetPostByIdAsync(int id);
        Task CreatePostAsync(Post post);
        Task DeletePostByIdAsync(int id);
        Task EditPostAsync(Post post);
    }
}
