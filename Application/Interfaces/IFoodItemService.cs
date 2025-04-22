using Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IFoodItemService
    {
        Task<IEnumerable<FoodItem>> GetAllFoodItemsAsync();
        Task<FoodItem> GetFoodItemByIdAsync(int id);
        Task<IEnumerable<FoodItem>> GetFoodItemsByCategoryAsync(int categoryId);
        Task<IEnumerable<FoodItem>> GetAvailableFoodItemsAsync();
        Task<FoodItem> CreateFoodItemAsync(FoodItem foodItem);
        Task UpdateFoodItemAsync(FoodItem foodItem);
        Task DeleteFoodItemAsync(int id);
        Task UpdateFoodItemStockAsync(int id, int newStock);
    }
}