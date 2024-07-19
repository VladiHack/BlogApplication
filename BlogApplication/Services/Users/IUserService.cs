using BlogApplication.Models;

namespace BlogApplication.Services.Users
{
    public interface IUserService
    {
        Task<IEnumerable<User>> GetUsersAsync();
        Task<bool> ExistsById(int id);
        Task<User> GetUserByIdAsync(int id);
        Task CreateUserAsync(User user);
        Task DeleteUserByIdAsync(int id);
        Task EditUserAsync(User user);

    }
}
