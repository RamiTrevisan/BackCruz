// Application/Services/FoodItemService.cs
using Application.Interfaces;
using Domain.Entities;
using Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Services
{
    public class FoodItemService : IFoodItemService
    {
        private readonly IFoodItemRepository _foodItemRepository;
        private readonly IFoodCategoryRepository _categoryRepository;

        public FoodItemService(
            IFoodItemRepository foodItemRepository,
            IFoodCategoryRepository categoryRepository)
        {
            _foodItemRepository = foodItemRepository;
            _categoryRepository = categoryRepository;
        }

        public async Task<IEnumerable<FoodItem>> GetAllFoodItemsAsync()
        {
            return await _foodItemRepository.GetAllAsync();
        }

        public async Task<FoodItem> GetFoodItemByIdAsync(int id)
        {
            return await _foodItemRepository.GetByIdAsync(id);
        }

        public async Task<IEnumerable<FoodItem>> GetFoodItemsByCategoryAsync(int categoryId)
        {
            // Verificar que la categoría existe
            var category = await _categoryRepository.GetByIdAsync(categoryId);
            if (category == null)
                throw new KeyNotFoundException($"Category with ID {categoryId} not found");

            return await _foodItemRepository.GetByCategoryAsync(categoryId);
        }

        public async Task<IEnumerable<FoodItem>> GetAvailableFoodItemsAsync()
        {
            return await _foodItemRepository.GetAvailableItemsAsync();
        }

        public async Task<FoodItem> CreateFoodItemAsync(FoodItem foodItem)
        {
            if (foodItem == null)
                throw new ArgumentNullException(nameof(foodItem));

            // Validaciones básicas
            if (string.IsNullOrWhiteSpace(foodItem.Name))
                throw new ArgumentException("Food item name is required", nameof(foodItem));

            if (foodItem.Price <= 0)
                throw new ArgumentException("Food item price must be greater than zero", nameof(foodItem));

            // Verificar que la categoría existe
            var category = await _categoryRepository.GetByIdAsync(foodItem.CategoryId);
            if (category == null)
                throw new KeyNotFoundException($"Category with ID {foodItem.CategoryId} not found");

            await _foodItemRepository.AddAsync(foodItem);
            return foodItem;
        }

        public async Task UpdateFoodItemAsync(FoodItem foodItem)
        {
            if (foodItem == null)
                throw new ArgumentNullException(nameof(foodItem));

            // Verificar que el ítem existe
            var existingItem = await _foodItemRepository.GetByIdAsync(foodItem.Id);
            if (existingItem == null)
                throw new KeyNotFoundException($"Food item with ID {foodItem.Id} not found");

            // Verificar que la categoría existe
            var category = await _categoryRepository.GetByIdAsync(foodItem.CategoryId);
            if (category == null)
                throw new KeyNotFoundException($"Category with ID {foodItem.CategoryId} not found");

            // Actualizar la disponibilidad basado en el stock
            foodItem.IsAvailable = foodItem.Stock > 0;

            await _foodItemRepository.UpdateAsync(foodItem);
        }

        public async Task DeleteFoodItemAsync(int id)
        {
            var foodItem = await _foodItemRepository.GetByIdAsync(id);
            if (foodItem == null)
                throw new KeyNotFoundException($"Food item with ID {id} not found");

            await _foodItemRepository.DeleteAsync(id);
        }

        public async Task UpdateFoodItemStockAsync(int id, int newStock)
        {
            if (newStock < 0)
                throw new ArgumentException("Stock cannot be negative", nameof(newStock));

            var foodItem = await _foodItemRepository.GetByIdAsync(id);
            if (foodItem == null)
                throw new KeyNotFoundException($"Food item with ID {id} not found");

            await _foodItemRepository.UpdateStockAsync(id, newStock);
        }
    }
}