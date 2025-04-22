using Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IMenuRepository : IBaseRepository<Menu>
    {
        Task<IEnumerable<Menu>> GetActiveMenusAsync();
        Task<Menu> GetMenuWithItemsAsync(int id);
        Task UpdateMenuStatusAsync(int id, bool isActive);
    }
}