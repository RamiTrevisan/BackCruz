using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IOrderRepository : IBaseRepository<Order>
    {
        Task<IEnumerable<Order>> GetOrdersByCustomerAsync(int customerId);
        Task<IEnumerable<Order>> GetOrdersByStatusAsync(OrderStatus status);
        Task<IEnumerable<Order>> GetOrdersByDateRangeAsync(DateTime start, DateTime end);
        Task<Order> GetOrderWithDetailsAsync(int orderId);
        Task UpdateOrderStatusAsync(int orderId, OrderStatus status);
    }
}