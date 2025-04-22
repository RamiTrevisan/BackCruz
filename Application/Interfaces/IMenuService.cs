using Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IMenuService
    {
        Task<IEnumerable<Menu>> GetAllMenusAsync();
        Task<Menu> GetMenuByIdAsync(int id);
        Task<Menu> GetMenuWithItemsAsync(int id);
        Task<IEnumerable<Menu>> GetActiveMenusAsync();
        Task<Menu> CreateMenuAsync(Menu menu);
        Task UpdateMenuAsync(Menu menu);
        Task DeleteMenuAsync(int id);
        Task UpdateMenuStatusAsync(int id, bool isActive);
        Task AddItemToMenuAsync(int menuId, int foodItemId, int quantity);
        Task RemoveItemFromMenuAsync(int menuId, int foodItemId);
    }
}