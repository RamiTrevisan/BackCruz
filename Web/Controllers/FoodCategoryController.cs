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
    public class FoodCategoryController : ControllerBase
    {
        private readonly IFoodCategoryService _categoryService;
        private readonly IMapper _mapper;

        public FoodCategoryController(IFoodCategoryService categoryService, IMapper mapper)
        {
            _categoryService = categoryService;
            _mapper = mapper;
        }

        // GET: api/FoodCategory
        [HttpGet]
        public async Task<ActionResult<IEnumerable<FoodCategoryDto>>> GetCategories()
        {
            var categories = await _categoryService.GetAllCategoriesAsync();
            return Ok(_mapper.Map<IEnumerable<FoodCategoryDto>>(categories));
        }

        // GET: api/FoodCategory/5
        [HttpGet("{id}")]
        public async Task<ActionResult<FoodCategoryDto>> GetCategory(int id)
        {
            var category = await _categoryService.GetCategoryByIdAsync(id);
            if (category == null)
                return NotFound();

            return Ok(_mapper.Map<FoodCategoryDto>(category));
        }

        // GET: api/FoodCategory/5/items
        [HttpGet("{id}/items")]
        public async Task<ActionResult<FoodCategoryDto>> GetCategoryWithItems(int id)
        {
            var category = await _categoryService.GetCategoryWithItemsAsync(id);
            if (category == null)
                return NotFound();

            return Ok(_mapper.Map<FoodCategoryDto>(category));
        }

        // POST: api/FoodCategory
        [HttpPost]
        [Authorize(Roles = "Admin")] // Descomentar cuando implementes autenticación
        public async Task<ActionResult<FoodCategoryDto>> CreateCategory(CreateFoodCategoryDto createCategoryDto)
        {
            try
            {
                // Mapear de DTO a entidad
                var category = _mapper.Map<FoodCategory>(createCategoryDto);

                // Inicializar la colección FoodItems con una lista vacía
                category.FoodItems = new List<FoodItem>();

                // Crear categoría
                var createdCategory = await _categoryService.CreateCategoryAsync(category);

                // Mapear respuesta
                var createdCategoryDto = _mapper.Map<FoodCategoryDto>(createdCategory);

                // Retornar respuesta con la ubicación del recurso creado
                return CreatedAtAction(nameof(GetCategory), new { id = createdCategoryDto.Id }, createdCategoryDto);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // PUT: api/FoodCategory/5
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")] // Descomentar cuando implementes autenticación
        public async Task<IActionResult> UpdateCategory(int id, FoodCategoryDto categoryDto)
        {
            if (id != categoryDto.Id)
                return BadRequest(new { message = "ID mismatch" });

            try
            {
                // Mapear de DTO a entidad
                var category = _mapper.Map<FoodCategory>(categoryDto);

                // Asegurarnos que FoodItems no sea null
                if (category.FoodItems == null)
                    category.FoodItems = new List<FoodItem>();

                // Actualizar categoría
                await _categoryService.UpdateCategoryAsync(category);

                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // DELETE: api/FoodCategory/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")] // Descomentar cuando implementes autenticación
        public async Task<IActionResult> DeleteCategory(int id)
        {
            try
            {
                await _categoryService.DeleteCategoryAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch (InvalidOperationException ex)
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