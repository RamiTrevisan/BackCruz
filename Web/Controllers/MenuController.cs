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
    public class MenuController : ControllerBase
    {
        private readonly IMenuService _menuService;
        private readonly IMapper _mapper;

        public MenuController(IMenuService menuService, IMapper mapper)
        {
            _menuService = menuService;
            _mapper = mapper;
        }

        // GET: api/Menu
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MenuDto>>> GetMenus()
        {
            var menus = await _menuService.GetAllMenusAsync();
            return Ok(_mapper.Map<IEnumerable<MenuDto>>(menus));
        }

        // GET: api/Menu/5
        [HttpGet("{id}")]
        public async Task<ActionResult<MenuDto>> GetMenu(int id)
        {
            var menu = await _menuService.GetMenuByIdAsync(id);
            if (menu == null)
                return NotFound();

            return Ok(_mapper.Map<MenuDto>(menu));
        }

        // GET: api/Menu/5/items
        [HttpGet("{id}/items")]
        public async Task<ActionResult<MenuDto>> GetMenuWithItems(int id)
        {
            var menu = await _menuService.GetMenuWithItemsAsync(id);
            if (menu == null)
                return NotFound();

            return Ok(_mapper.Map<MenuDto>(menu));
        }

        // GET: api/Menu/active
        [HttpGet("active")]
        public async Task<ActionResult<IEnumerable<MenuDto>>> GetActiveMenus()
        {
            var menus = await _menuService.GetActiveMenusAsync();
            return Ok(_mapper.Map<IEnumerable<MenuDto>>(menus));
        }

        // POST: api/Menu
        [HttpPost]
        [Authorize(Roles = "Admin")] // Descomentar cuando implementes autenticación
        public async Task<ActionResult<MenuDto>> CreateMenu(CreateMenuDto createMenuDto)
        {
            try
            {
                // Mapear de DTO a entidad
                var menu = _mapper.Map<Menu>(createMenuDto);

                // Inicializar colecciones
                menu.MenuItems = new List<MenuFoodItem>();
                menu.OrderDetails = new List<OrderDetail>();

                // Crear menú
                var createdMenu = await _menuService.CreateMenuAsync(menu);

                // Mapear respuesta
                var createdMenuDto = _mapper.Map<MenuDto>(createdMenu);

                // Retornar respuesta con la ubicación del recurso creado
                return CreatedAtAction(nameof(GetMenu), new { id = createdMenuDto.Id }, createdMenuDto);
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

        // PUT: api/Menu/5
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")] // Descomentar cuando implementes autenticación
        public async Task<IActionResult> UpdateMenu(int id, MenuDto menuDto)
        {
            if (id != menuDto.Id)
                return BadRequest(new { message = "ID mismatch" });

            try
            {
                // Mapear de DTO a entidad
                var menu = _mapper.Map<Menu>(menuDto);

                // Asegurarnos que las colecciones no sean null
                if (menu.MenuItems == null)
                    menu.MenuItems = new List<MenuFoodItem>();

                if (menu.OrderDetails == null)
                    menu.OrderDetails = new List<OrderDetail>();

                // Actualizar menú
                await _menuService.UpdateMenuAsync(menu);

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

        // DELETE: api/Menu/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")] // Descomentar cuando implementes autenticación
        public async Task<IActionResult> DeleteMenu(int id)
        {
            try
            {
                await _menuService.DeleteMenuAsync(id);
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

        // PATCH: api/Menu/5/status/true
        [HttpPatch("{id}/status/{isActive}")]
        [Authorize(Roles = "Admin")] // Descomentar cuando implementes autenticación
        public async Task<IActionResult> UpdateMenuStatus(int id, bool isActive)
        {
            try
            {
                await _menuService.UpdateMenuStatusAsync(id, isActive);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
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

        // Reemplaza este método en MenuController.cs

        // POST: api/Menu/5/items
        [HttpPost("{menuId}/items")]
        [Authorize(Roles = "Admin")] 
        public async Task<IActionResult> AddItemToMenu(int menuId, [FromBody] AddMenuItemDto addMenuItemDto)
        {
            if (menuId != addMenuItemDto.MenuId)
                return BadRequest(new { message = "Menu ID mismatch" });

            try
            {
                await _menuService.AddItemToMenuAsync(menuId, addMenuItemDto.FoodItemId, addMenuItemDto.Quantity);
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

        // DELETE: api/Menu/5/items/2
        [HttpDelete("{menuId}/items/{foodItemId}")]
        [Authorize(Roles = "Admin")] // Descomentar cuando implementes autenticación
        public async Task<IActionResult> RemoveItemFromMenu(int menuId, int foodItemId)
        {
            try
            {
                await _menuService.RemoveItemFromMenuAsync(menuId, foodItemId);
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
    }
}