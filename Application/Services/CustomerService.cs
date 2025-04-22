// Application/Services/CustomerService.cs
using Application.Interfaces;
using Domain.Entities;
using Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository _customerRepository;

        public CustomerService(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        public async Task<IEnumerable<Customer>> GetAllCustomersAsync()
        {
            return await _customerRepository.GetAllAsync();
        }

        public async Task<Customer> GetCustomerByIdAsync(int id)
        {
            return await _customerRepository.GetByIdAsync(id);
        }

        public async Task<Customer> GetCustomerWithOrdersAsync(int id)
        {
            return await _customerRepository.GetWithOrdersAsync(id);
        }

        public async Task<Customer> CreateCustomerAsync(Customer customer)
        {
            if (customer == null)
                throw new ArgumentNullException(nameof(customer));

            // Validación básica
            if (string.IsNullOrWhiteSpace(customer.FullName))
                throw new ArgumentException("Customer name is required", nameof(customer));

            await _customerRepository.AddAsync(customer);
            return customer;
        }

        public async Task UpdateCustomerAsync(Customer customer)
        {
            if (customer == null)
                throw new ArgumentNullException(nameof(customer));

            // Verificar que el cliente existe
            var existingCustomer = await _customerRepository.GetByIdAsync(customer.Id);
            if (existingCustomer == null)
                throw new KeyNotFoundException($"Customer with ID {customer.Id} not found");

            // Actualizar propiedades
            await _customerRepository.UpdateAsync(customer);
        }

        public async Task DeleteCustomerAsync(int id)
        {
            var customer = await _customerRepository.GetByIdAsync(id);
            if (customer == null)
                throw new KeyNotFoundException($"Customer with ID {id} not found");

            // Verificar si tiene órdenes activas
            var customerWithOrders = await _customerRepository.GetWithOrdersAsync(id);
            if (customerWithOrders.Orders != null && customerWithOrders.Orders.Count > 0)
            {
                throw new InvalidOperationException($"Cannot delete customer with ID {id} because they have orders");
            }

            await _customerRepository.DeleteAsync(id);
        }

        public async Task<IEnumerable<Customer>> GetTopCustomersAsync(int count)
        {
            if (count <= 0)
                throw new ArgumentException("Count must be greater than zero", nameof(count));

            return await _customerRepository.GetTopCustomersAsync(count);
        }
    }
}