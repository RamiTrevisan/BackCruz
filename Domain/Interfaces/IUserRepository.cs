using Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IUserRepository : IBaseRepository<User>
    {
        Task<User> GetByUsernameAsync(string username);
        Task<User> AuthenticateAsync(string username, string password);
        Task<IEnumerable<User>> GetUsersByRoleAsync(int roleId);
    }
}