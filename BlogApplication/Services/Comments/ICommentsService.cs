using BlogApplication.Models;

namespace BlogApplication.Services.Comments
{
    public interface ICommentsService
    {
        Task<IEnumerable<Comment>> GetCommentsAsync();
        Task<bool> ExistsById(int id);
        Task<Comment> GetCommentByIdAsync(int id);
        Task CreateCommentAsync(Comment comment);
        Task DeleteCommentByIdAsync(int id);
        Task EditCommentAsync(Comment comment);
    }
}
