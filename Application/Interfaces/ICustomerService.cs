using Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface ICustomerService
    {
        Task<IEnumerable<Customer>> GetAllCustomersAsync();
        Task<Customer> GetCustomerByIdAsync(int id);
        Task<Customer> GetCustomerWithOrdersAsync(int id);
        Task<Customer> CreateCustomerAsync(Customer customer);
        Task UpdateCustomerAsync(Customer customer);
        Task DeleteCustomerAsync(int id);
        Task<IEnumerable<Customer>> GetTopCustomersAsync(int count);
    }
}