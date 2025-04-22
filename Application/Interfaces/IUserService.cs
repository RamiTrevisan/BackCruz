using Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IUserService
    {
        Task<IEnumerable<User>> GetAllUsersAsync();
        Task<User> GetUserByIdAsync(int id);
        Task<User> AuthenticateAsync(string username, string password);
        Task<User> CreateUserAsync(User user, string password);
        Task UpdateUserAsync(User user, string password = null);
        Task DeleteUserAsync(int id);
    }
}