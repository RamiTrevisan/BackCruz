using Domain.Entities;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Infrastructure.Data
{
    public class FoodCategoryRepository : BaseRepository<FoodCategory>, IFoodCategoryRepository
    {
        public FoodCategoryRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<FoodCategory> GetWithFoodItemsAsync(int categoryId)
        {
            return await _dbSet
                .Include(c => c.FoodItems)
                .FirstOrDefaultAsync(c => c.Id == categoryId);
        }

        public override async Task<IEnumerable<FoodCategory>> GetAllAsync()
        {
            return await _dbSet
                .Include(c => c.FoodItems)
                .ToListAsync();
        }
    }
}