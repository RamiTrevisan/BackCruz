using Domain.Entities;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Data
{
    public class MenuRepository : BaseRepository<Menu>, IMenuRepository
    {
        public MenuRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Menu>> GetActiveMenusAsync()
        {
            return await _dbSet
                .Where(m => m.IsActive && m.Stock > 0)
                .ToListAsync();
        }

        public async Task<Menu> GetMenuWithItemsAsync(int id)
        {
            return await _dbSet
                .Include(m => m.MenuItems)
                    .ThenInclude(mi => mi.FoodItem)
                .FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task UpdateMenuStatusAsync(int id, bool isActive)
        {
            var menu = await _dbSet.FindAsync(id);
            if (menu != null)
            {
                menu.IsActive = isActive;
                await _context.SaveChangesAsync();
            }
        }

        public override async Task<IEnumerable<Menu>> GetAllAsync()
        {
            return await _dbSet
                .Include(m => m.MenuItems)
                    .ThenInclude(mi => mi.FoodItem)
                .ToListAsync();
        }
    }
}