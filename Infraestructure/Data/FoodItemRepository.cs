using Domain.Entities;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Data
{
    public class FoodItemRepository : BaseRepository<FoodItem>, IFoodItemRepository
    {
        public FoodItemRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<FoodItem>> GetByCategoryAsync(int categoryId)
        {
            return await _dbSet
                .Where(f => f.CategoryId == categoryId)
                .ToListAsync();
        }

        public async Task<IEnumerable<FoodItem>> GetAvailableItemsAsync()
        {
            return await _dbSet
                .Where(f => f.IsAvailable && f.Stock > 0)
                .Include(f => f.Category)
                .ToListAsync();
        }

        public async Task UpdateStockAsync(int id, int newStock)
        {
            var foodItem = await _dbSet.FindAsync(id);
            if (foodItem != null)
            {
                foodItem.Stock = newStock;
                foodItem.IsAvailable = newStock > 0;
                await _context.SaveChangesAsync();
            }
        }

        public override async Task<FoodItem> GetByIdAsync(int id)
        {
            return await _dbSet
                .Include(f => f.Category)
                .FirstOrDefaultAsync(f => f.Id == id);
        }

        public override async Task<IEnumerable<FoodItem>> GetAllAsync()
        {
            return await _dbSet
                .Include(f => f.Category)
                .ToListAsync();
        }
    }
}
