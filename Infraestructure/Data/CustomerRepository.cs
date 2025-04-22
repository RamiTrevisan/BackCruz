using Domain.Entities;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Data
{
    public class CustomerRepository : BaseRepository<Customer>, ICustomerRepository
    {
        public CustomerRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<Customer> GetWithOrdersAsync(int id)
        {
            return await _dbSet
                .Include(c => c.Orders)
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<IEnumerable<Customer>> GetTopCustomersAsync(int count)
        {
            return await _dbSet
                .Include(c => c.Orders)
                .OrderByDescending(c => c.Orders.Count)
                .Take(count)
                .ToListAsync();
        }

        public override async Task<Customer> GetByIdAsync(int id)
        {
            return await _dbSet
                .Include(c => c.User)
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public override async Task<IEnumerable<Customer>> GetAllAsync()
        {
            return await _dbSet
                .Include(c => c.User)
                .ToListAsync();
        }
    }
}