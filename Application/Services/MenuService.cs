// Application/Services/MenuService.cs
using Application.Interfaces;
using Domain.Entities;
using Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Services
{
    public class MenuService : IMenuService
    {
        private readonly IMenuRepository _menuRepository;
        private readonly IFoodItemRepository _foodItemRepository;

        public MenuService(
            IMenuRepository menuRepository,
            IFoodItemRepository foodItemRepository)
        {
            _menuRepository = menuRepository;
            _foodItemRepository = foodItemRepository;
        }

        public async Task<IEnumerable<Menu>> GetAllMenusAsync()
        {
            return await _menuRepository.GetAllAsync();
        }

        public async Task<Menu> GetMenuByIdAsync(int id)
        {
            return await _menuRepository.GetByIdAsync(id);
        }

        public async Task<Menu> GetMenuWithItemsAsync(int id)
        {
            return await _menuRepository.GetMenuWithItemsAsync(id);
        }

        public async Task<IEnumerable<Menu>> GetActiveMenusAsync()
        {
            return await _menuRepository.GetActiveMenusAsync();
        }

        public async Task<Menu> CreateMenuAsync(Menu menu)
        {
            if (menu == null)
                throw new ArgumentNullException(nameof(menu));

            // Validaciones básicas
            if (string.IsNullOrWhiteSpace(menu.Name))
                throw new ArgumentException("Menu name is required", nameof(menu));

            if (menu.Price <= 0)
                throw new ArgumentException("Menu price must be greater than zero", nameof(menu));

            // Establecer valores predeterminados
            menu.ValidFrom = menu.ValidFrom == default ? DateTime.Now : menu.ValidFrom;
            menu.IsActive = menu.Stock > 0;

            await _menuRepository.AddAsync(menu);
            return menu;
        }

        public async Task UpdateMenuAsync(Menu menu)
        {
            if (menu == null)
                throw new ArgumentNullException(nameof(menu));

            // Verificar que el menú existe
            var existingMenu = await _menuRepository.GetByIdAsync(menu.Id);
            if (existingMenu == null)
                throw new KeyNotFoundException($"Menu with ID {menu.Id} not found");

            // Actualizar la disponibilidad basado en el stock
            menu.IsActive = menu.IsActive && menu.Stock > 0;

            await _menuRepository.UpdateAsync(menu);
        }

        public async Task DeleteMenuAsync(int id)
        {
            var menu = await _menuRepository.GetByIdAsync(id);
            if (menu == null)
                throw new KeyNotFoundException($"Menu with ID {id} not found");

            await _menuRepository.DeleteAsync(id);
        }

        public async Task UpdateMenuStatusAsync(int id, bool isActive)
        {
            var menu = await _menuRepository.GetByIdAsync(id);
            if (menu == null)
                throw new KeyNotFoundException($"Menu with ID {id} not found");

            // Si queremos activar el menú, verificamos que tenga stock
            if (isActive && menu.Stock <= 0)
                throw new InvalidOperationException($"Cannot activate menu with ID {id} because it has no stock");

            await _menuRepository.UpdateMenuStatusAsync(id, isActive);
        }

        public async Task AddItemToMenuAsync(int menuId, int foodItemId, int quantity)
        {
            if (quantity <= 0)
                throw new ArgumentException("Quantity must be greater than zero", nameof(quantity));

            // Verificar que el menú existe
            var menu = await _menuRepository.GetMenuWithItemsAsync(menuId);
            if (menu == null)
                throw new KeyNotFoundException($"Menu with ID {menuId} not found");

            // Verificar que el ítem de comida existe
            var foodItem = await _foodItemRepository.GetByIdAsync(foodItemId);
            if (foodItem == null)
                throw new KeyNotFoundException($"Food item with ID {foodItemId} not found");

            // Verificar si el ítem ya está en el menú
            var existingItem = menu.MenuItems?.FirstOrDefault(mi => mi.FoodItemId == foodItemId);

            if (existingItem != null)
            {
                // Actualizar la cantidad
                existingItem.Quantity = quantity;
                await _menuRepository.UpdateAsync(menu);
            }
            else
            {
                // Agregar nuevo ítem al menú
                if (menu.MenuItems == null)
                    menu.MenuItems = new List<MenuFoodItem>();

                menu.MenuItems.Add(new MenuFoodItem
                {
                    MenuId = menuId,
                    FoodItemId = foodItemId,
                    Quantity = quantity
                });

                await _menuRepository.UpdateAsync(menu);
            }
        }

        public async Task RemoveItemFromMenuAsync(int menuId, int foodItemId)
        {
            // Verificar que el menú existe
            var menu = await _menuRepository.GetMenuWithItemsAsync(menuId);
            if (menu == null)
                throw new KeyNotFoundException($"Menu with ID {menuId} not found");

            // Verificar si el ítem está en el menú
            var menuItem = menu.MenuItems?.FirstOrDefault(mi => mi.FoodItemId == foodItemId);
            if (menuItem == null)
                throw new KeyNotFoundException($"Food item with ID {foodItemId} not found in menu with ID {menuId}");

            // Remover el ítem del menú
            menu.MenuItems.Remove(menuItem);
            await _menuRepository.UpdateAsync(menu);
        }
    }
}