using Application.DTOs;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FoodItemController : ControllerBase
    {
        private readonly IFoodItemService _foodItemService;
        private readonly IMapper _mapper;

        public FoodItemController(IFoodItemService foodItemService, IMapper mapper)
        {
            _foodItemService = foodItemService;
            _mapper = mapper;
        }

        // GET: api/FoodItem
        [HttpGet]
        public async Task<ActionResult<IEnumerable<FoodItemDto>>> GetFoodItems()
        {
            var foodItems = await _foodItemService.GetAllFoodItemsAsync();
            return Ok(_mapper.Map<IEnumerable<FoodItemDto>>(foodItems));
        }

        // GET: api/FoodItem/5
        [HttpGet("{id}")]
        public async Task<ActionResult<FoodItemDto>> GetFoodItem(int id)
        {
            var foodItem = await _foodItemService.GetFoodItemByIdAsync(id);
            if (foodItem == null)
                return NotFound();

            return Ok(_mapper.Map<FoodItemDto>(foodItem));
        }

        // GET: api/FoodItem/category/5
        [HttpGet("category/{categoryId}")]
        public async Task<ActionResult<IEnumerable<FoodItemDto>>> GetFoodItemsByCategory(int categoryId)
        {
            try
            {
                var foodItems = await _foodItemService.GetFoodItemsByCategoryAsync(categoryId);
                return Ok(_mapper.Map<IEnumerable<FoodItemDto>>(foodItems));
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new { message = $"Category with ID {categoryId} not found" });
            }
        }

        // GET: api/FoodItem/available
        [HttpGet("available")]
        public async Task<ActionResult<IEnumerable<FoodItemDto>>> GetAvailableFoodItems()
        {
            var foodItems = await _foodItemService.GetAvailableFoodItemsAsync();
            return Ok(_mapper.Map<IEnumerable<FoodItemDto>>(foodItems));
        }

        // POST: api/FoodItem
        [HttpPost]
        [Authorize(Roles = "Admin")] // Descomentar cuando implementes autenticación
        public async Task<ActionResult<FoodItemDto>> CreateFoodItem(CreateFoodItemDto createFoodItemDto)
        {
            try
            {
                // Mapear de DTO a entidad
                var foodItem = _mapper.Map<FoodItem>(createFoodItemDto);

                // Inicializar la colección MenuItems con una lista vacía
                foodItem.MenuItems = new List<MenuFoodItem>();

                // Crear artículo de comida
                var createdFoodItem = await _foodItemService.CreateFoodItemAsync(foodItem);

                // Mapear respuesta
                var createdFoodItemDto = _mapper.Map<FoodItemDto>(createdFoodItem);

                // Retornar respuesta con la ubicación del recurso creado
                return CreatedAtAction(nameof(GetFoodItem), new { id = createdFoodItemDto.Id }, createdFoodItemDto);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // PUT: api/FoodItem/5
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")] // Descomentar cuando implementes autenticación
        public async Task<IActionResult> UpdateFoodItem(int id, FoodItemDto foodItemDto)
        {
            if (id != foodItemDto.Id)
                return BadRequest(new { message = "ID mismatch" });

            try
            {
                // Mapear de DTO a entidad
                var foodItem = _mapper.Map<FoodItem>(foodItemDto);

                // Asegurarnos que MenuItems no sea null
                if (foodItem.MenuItems == null)
                    foodItem.MenuItems = new List<MenuFoodItem>();

                // Actualizar artículo de comida
                await _foodItemService.UpdateFoodItemAsync(foodItem);

                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // DELETE: api/FoodItem/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")] // Descomentar cuando implementes autenticación
        public async Task<IActionResult> DeleteFoodItem(int id)
        {
            try
            {
                await _foodItemService.DeleteFoodItemAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // PATCH: api/FoodItem/5/stock/10
        [HttpPatch("{id}/stock/{newStock}")]
        [Authorize(Roles = "Admin")] // Descomentar cuando implementes autenticación
        public async Task<IActionResult> UpdateFoodItemStock(int id, int newStock)
        {
            try
            {
                await _foodItemService.UpdateFoodItemStockAsync(id, newStock);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}