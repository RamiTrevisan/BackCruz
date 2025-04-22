using Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IFoodCategoryRepository : IBaseRepository<FoodCategory>
    {
        Task<FoodCategory> GetWithFoodItemsAsync(int categoryId);
    }
}

