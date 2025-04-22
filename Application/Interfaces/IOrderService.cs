using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IOrderService
    {
        Task<IEnumerable<Order>> GetAllOrdersAsync();
        Task<Order> GetOrderByIdAsync(int id);
        Task<Order> GetOrderWithDetailsAsync(int id);
        Task<IEnumerable<Order>> GetOrdersByCustomerAsync(int customerId);
        Task<IEnumerable<Order>> GetOrdersByStatusAsync(OrderStatus status);
        Task<IEnumerable<Order>> GetOrdersByDateRangeAsync(DateTime start, DateTime end);
        Task<Order> CreateOrderAsync(Order order);
        Task UpdateOrderAsync(Order order);
        Task DeleteOrderAsync(int id);
        Task UpdateOrderStatusAsync(int orderId, OrderStatus status);
        Task<decimal> CalculateOrderTotalAsync(int orderId);
    }
}