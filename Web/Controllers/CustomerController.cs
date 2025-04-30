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
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerService _customerService;
        private readonly IMapper _mapper;

        public CustomerController(ICustomerService customerService, IMapper mapper)
        {
            _customerService = customerService;
            _mapper = mapper;
        }

        // GET: api/Customer
        [HttpGet]
        [Authorize] // Descomentar cuando implementes autenticación
        public async Task<ActionResult<IEnumerable<CustomerDto>>> GetCustomers()
        {
            var customers = await _customerService.GetAllCustomersAsync();
            return Ok(_mapper.Map<IEnumerable<CustomerDto>>(customers));
        }

        // GET: api/Customer/5
        [HttpGet("{id}")]
        [Authorize] // Descomentar cuando implementes autenticación
        public async Task<ActionResult<CustomerDto>> GetCustomer(int id)
        {
            var customer = await _customerService.GetCustomerByIdAsync(id);
            if (customer == null)
                return NotFound();

            return Ok(_mapper.Map<CustomerDto>(customer));
        }

        // GET: api/Customer/5/orders
        [HttpGet("{id}/orders")]
        [Authorize] // Descomentar cuando implementes autenticación
        public async Task<ActionResult<CustomerDto>> GetCustomerWithOrders(int id)
        {
            var customer = await _customerService.GetCustomerWithOrdersAsync(id);
            if (customer == null)
                return NotFound();

            return Ok(_mapper.Map<CustomerDto>(customer));
        }

        // POST: api/Customer
        [HttpPost]
        [Authorize] // Descomentar cuando implementes autenticación
        public async Task<ActionResult<CustomerDto>> CreateCustomer(CreateCustomerDto createCustomerDto)
        {
            try
            {
                // Mapear de DTO a entidad
                var customer = _mapper.Map<Customer>(createCustomerDto);

                // Inicializar la colección Orders con una lista vacía
                customer.Orders = new List<Order>();

                // Crear cliente
                var createdCustomer = await _customerService.CreateCustomerAsync(customer);

                // Mapear la respuesta
                var createdCustomerDto = _mapper.Map<CustomerDto>(createdCustomer);

                // Retornar respuesta con la ubicación del recurso creado
                return CreatedAtAction(nameof(GetCustomer), new { id = createdCustomerDto.Id }, createdCustomerDto);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // PUT: api/Customer/5
        [HttpPut("{id}")]
        [Authorize] // Descomentar cuando implementes autenticación
        public async Task<IActionResult> UpdateCustomer(int id, CustomerDto customerDto)
        {
            if (id != customerDto.Id)
                return BadRequest(new { message = "ID mismatch" });

            try
            {
                // Mapear de DTO a entidad
                var customer = _mapper.Map<Customer>(customerDto);

                // Asegurarnos que Orders no sea null
                if (customer.Orders == null)
                    customer.Orders = new List<Order>();

                // Actualizar cliente
                await _customerService.UpdateCustomerAsync(customer);

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

        // DELETE: api/Customer/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")] // Descomentar cuando implementes autenticación
        public async Task<IActionResult> DeleteCustomer(int id)
        {
            try
            {
                await _customerService.DeleteCustomerAsync(id);
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

        // GET: api/Customer/top/5
        [HttpGet("top/{count}")]
        [Authorize(Roles = "Admin")] // Descomentar cuando implementes autenticación
        public async Task<ActionResult<IEnumerable<CustomerDto>>> GetTopCustomers(int count)
        {
            try
            {
                var customers = await _customerService.GetTopCustomersAsync(count);
                return Ok(_mapper.Map<IEnumerable<CustomerDto>>(customers));
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}