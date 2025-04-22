// Application/Services/FoodCategoryService.cs
using Application.Interfaces;
using Domain.Entities;
using Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Services
{
    public class FoodCategoryService : IFoodCategoryService
    {
        private readonly IFoodCategoryRepository _categoryRepository;

        public FoodCategoryService(IFoodCategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<IEnumerable<FoodCategory>> GetAllCategoriesAsync()
        {
            return await _categoryRepository.GetAllAsync();
        }

        public async Task<FoodCategory> GetCategoryByIdAsync(int id)
        {
            return await _categoryRepository.GetByIdAsync(id);
        }

        public async Task<FoodCategory> GetCategoryWithItemsAsync(int id)
        {
            return await _categoryRepository.GetWithFoodItemsAsync(id);
        }

        public async Task<FoodCategory> CreateCategoryAsync(FoodCategory category)
        {
            if (category == null)
                throw new ArgumentNullException(nameof(category));

            // Validación básica
            if (string.IsNullOrWhiteSpace(category.Name))
                throw new ArgumentException("Category name is required", nameof(category));

            await _categoryRepository.AddAsync(category);
            return category;
        }

        public async Task UpdateCategoryAsync(FoodCategory category)
        {
            if (category == null)
                throw new ArgumentNullException(nameof(category));

            // Verificar que la categoría existe
            var existingCategory = await _categoryRepository.GetByIdAsync(category.Id);
            if (existingCategory == null)
                throw new KeyNotFoundException($"Category with ID {category.Id} not found");

            await _categoryRepository.UpdateAsync(category);
        }

        public async Task DeleteCategoryAsync(int id)
        {
            var category = await _categoryRepository.GetWithFoodItemsAsync(id);
            if (category == null)
                throw new KeyNotFoundException($"Category with ID {id} not found");

            // Verificar si tiene productos asociados
            if (category.FoodItems != null && category.FoodItems.Count > 0)
            {
                throw new InvalidOperationException($"Cannot delete category with ID {id} because it has associated food items");
            }

            await _categoryRepository.DeleteAsync(id);
        }
    }
}