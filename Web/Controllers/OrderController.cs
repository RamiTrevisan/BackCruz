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
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;
        private readonly IMapper _mapper;

        public OrderController(IOrderService orderService, IMapper mapper)
        {
            _orderService = orderService;
            _mapper = mapper;
        }

        // GET: api/Order
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<OrderDto>>> GetOrders()
        {
            var orders = await _orderService.GetAllOrdersAsync();
            return Ok(_mapper.Map<IEnumerable<OrderDto>>(orders));
        }

        // GET: api/Order/5
        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<OrderDto>> GetOrder(int id)
        {
            var order = await _orderService.GetOrderByIdAsync(id);
            if (order == null)
                return NotFound();

            return Ok(_mapper.Map<OrderDto>(order));
        }

        // GET: api/Order/5/details
        [HttpGet("{id}/details")]
        [Authorize]
        public async Task<ActionResult<OrderDto>> GetOrderWithDetails(int id)
        {
            var order = await _orderService.GetOrderWithDetailsAsync(id);
            if (order == null)
                return NotFound();

            return Ok(_mapper.Map<OrderDto>(order));
        }

        // GET: api/Order/customer/5
        [HttpGet("customer/{customerId}")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<OrderDto>>> GetOrdersByCustomer(int customerId)
        {
            try
            {
                var orders = await _orderService.GetOrdersByCustomerAsync(customerId);
                return Ok(_mapper.Map<IEnumerable<OrderDto>>(orders));
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        // GET: api/Order/status/1
        [HttpGet("status/{status}")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<OrderDto>>> GetOrdersByStatus(OrderStatus status)
        {
            var orders = await _orderService.GetOrdersByStatusAsync(status);
            return Ok(_mapper.Map<IEnumerable<OrderDto>>(orders));
        }

        // GET: api/Order/date?start=2023-01-01&end=2023-12-31
        [HttpGet("date")]
        [Authorize] //
        public async Task<ActionResult<IEnumerable<OrderDto>>> GetOrdersByDateRange([FromQuery] DateTime start, [FromQuery] DateTime end)
        {
            try
            {
                var orders = await _orderService.GetOrdersByDateRangeAsync(start, end);
                return Ok(_mapper.Map<IEnumerable<OrderDto>>(orders));
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
        // Reemplaza este método en OrderController.cs

        // POST: api/Order
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<OrderDto>> CreateOrder(CreateOrderDto createOrderDto)
        {
            try
            {
                // Mapear de DTO a entidad
                var order = _mapper.Map<Order>(createOrderDto);

                // Asegurarnos que OrderDetails no sea null
                if (order.OrderDetails == null && createOrderDto.OrderDetails != null)
                {
                    order.OrderDetails = _mapper.Map<List<OrderDetail>>(createOrderDto.OrderDetails);
                }

                // Crear pedido
                var createdOrder = await _orderService.CreateOrderAsync(order);

                // Mapear respuesta
                var createdOrderDto = _mapper.Map<OrderDto>(createdOrder);

                // Retornar respuesta con la ubicación del recurso creado
                return CreatedAtAction(nameof(GetOrder), new { id = createdOrderDto.Id }, createdOrderDto);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
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

        // PUT: api/Order/5
        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> UpdateOrder(int id, OrderDto orderDto)
        {
            if (id != orderDto.Id)
                return BadRequest(new { message = "ID mismatch" });

            try
            {
                // Mapear de DTO a entidad
                var order = _mapper.Map<Order>(orderDto);

                // Actualizar pedido
                await _orderService.UpdateOrderAsync(order);

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

        // DELETE: api/Order/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            try
            {
                await _orderService.DeleteOrderAsync(id);
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

        // PATCH: api/Order/5/status/2
        [HttpPatch("{id}/status/{status}")]
        [Authorize]
        public async Task<IActionResult> UpdateOrderStatus(int id, OrderStatus status)
        {
            try
            {
                await _orderService.UpdateOrderStatusAsync(id, status);
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

        // GET: api/Order/5/total
        [HttpGet("{id}/total")]
        [Authorize] 
        public async Task<ActionResult<decimal>> CalculateOrderTotal(int id)
        {
            try
            {
                var total = await _orderService.CalculateOrderTotalAsync(id);
                return Ok(new { total });
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