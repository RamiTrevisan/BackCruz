using Domain.Entities;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Data
{
    public class OrderRepository : BaseRepository<Order>, IOrderRepository
    {
        public OrderRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Order>> GetOrdersByCustomerAsync(int customerId)
        {
            return await _dbSet
                .Where(o => o.CustomerId == customerId)
                .Include(o => o.OrderDetails)
                .OrderByDescending(o => o.OrderDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<Order>> GetOrdersByStatusAsync(OrderStatus status)
        {
            return await _dbSet
                .Where(o => o.Status == status)
                .Include(o => o.Customer)
                .Include(o => o.OrderDetails)
                .OrderByDescending(o => o.OrderDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<Order>> GetOrdersByDateRangeAsync(DateTime start, DateTime end)
        {
            return await _dbSet
                .Where(o => o.OrderDate >= start && o.OrderDate <= end)
                .Include(o => o.Customer)
                .OrderByDescending(o => o.OrderDate)
                .ToListAsync();
        }

        public async Task<Order> GetOrderWithDetailsAsync(int orderId)
        {
            return await _dbSet
                .Include(o => o.Customer)
                .Include(o => o.OrderDetails)
                    .ThenInclude(od => od.Menu)
                .FirstOrDefaultAsync(o => o.Id == orderId);
        }

        public async Task UpdateOrderStatusAsync(int orderId, OrderStatus status)
        {
            var order = await _dbSet.FindAsync(orderId);
            if (order != null)
            {
                order.Status = status;
                await _context.SaveChangesAsync();
            }
        }

        public override async Task<IEnumerable<Order>> GetAllAsync()
        {
            return await _dbSet
                .Include(o => o.Customer)
                .Include(o => o.OrderDetails)
                .OrderByDescending(o => o.OrderDate)
                .ToListAsync();
        }

        public override async Task<Order> GetByIdAsync(int id)
        {
            return await _dbSet
                .Include(o => o.Customer)
                .Include(o => o.OrderDetails)
                .FirstOrDefaultAsync(o => o.Id == id);
        }
    }
}