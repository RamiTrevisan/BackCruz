using Domain.Entities;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Data
{
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        public UserRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<User> GetByUsernameAsync(string username)
        {
            return await _dbSet
                .Include(u => u.Role)
                .FirstOrDefaultAsync(u => u.Username == username);
        }

        public async Task<User> AuthenticateAsync(string username, string password)
        {
            return await _dbSet
                .Include(u => u.Role)
                .FirstOrDefaultAsync(u => u.Username == username && u.Password == password);
        }

        public async Task<IEnumerable<User>> GetUsersByRoleAsync(int roleId)
        {
            return await _dbSet
                .Include(u => u.Role)
                .Where(u => u.RoleId == roleId)
                .ToListAsync();
        }

        public override async Task<User> GetByIdAsync(int id)
        {
            return await _dbSet
                .Include(u => u.Role)
                .FirstOrDefaultAsync(u => u.Id == id);
        }
    }
}
