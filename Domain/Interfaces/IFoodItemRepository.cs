using Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IFoodItemRepository : IBaseRepository<FoodItem>
    {
        Task<IEnumerable<FoodItem>> GetByCategoryAsync(int categoryId);
        Task<IEnumerable<FoodItem>> GetAvailableItemsAsync();
        Task UpdateStockAsync(int id, int newStock);
    }
}