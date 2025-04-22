using Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface ICustomerRepository : IBaseRepository<Customer>
    {
        Task<Customer> GetWithOrdersAsync(int id);
        Task<IEnumerable<Customer>> GetTopCustomersAsync(int count);
    }
}