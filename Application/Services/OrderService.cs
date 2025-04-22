// Application/Services/OrderService.cs
using Application.Interfaces;
using Domain.Entities;
using Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly ICustomerRepository _customerRepository;
        private readonly IMenuRepository _menuRepository;

        public OrderService(
            IOrderRepository orderRepository,
            ICustomerRepository customerRepository,
            IMenuRepository menuRepository)
        {
            _orderRepository = orderRepository;
            _customerRepository = customerRepository;
            _menuRepository = menuRepository;
        }

        public async Task<IEnumerable<Order>> GetAllOrdersAsync()
        {
            return await _orderRepository.GetAllAsync();
        }

        public async Task<Order> GetOrderByIdAsync(int id)
        {
            return await _orderRepository.GetByIdAsync(id);
        }

        public async Task<Order> GetOrderWithDetailsAsync(int id)
        {
            return await _orderRepository.GetOrderWithDetailsAsync(id);
        }

        public async Task<IEnumerable<Order>> GetOrdersByCustomerAsync(int customerId)
        {
            // Verificar que el cliente existe
            var customer = await _customerRepository.GetByIdAsync(customerId);
            if (customer == null)
                throw new KeyNotFoundException($"Customer with ID {customerId} not found");

            return await _orderRepository.GetOrdersByCustomerAsync(customerId);
        }

        public async Task<IEnumerable<Order>> GetOrdersByStatusAsync(OrderStatus status)
        {
            return await _orderRepository.GetOrdersByStatusAsync(status);
        }

        public async Task<IEnumerable<Order>> GetOrdersByDateRangeAsync(DateTime start, DateTime end)
        {
            if (start > end)
                throw new ArgumentException("Start date must be before end date");

            return await _orderRepository.GetOrdersByDateRangeAsync(start, end);
        }

        public async Task<Order> CreateOrderAsync(Order order)
        {
            if (order == null)
                throw new ArgumentNullException(nameof(order));

            // Verificar que el cliente existe
            var customer = await _customerRepository.GetByIdAsync(order.CustomerId);
            if (customer == null)
                throw new KeyNotFoundException($"Customer with ID {order.CustomerId} not found");

            // Verificar detalles del pedido
            if (order.OrderDetails == null || !order.OrderDetails.Any())
                throw new ArgumentException("Order must have at least one detail");

            // Establecer valores predeterminados
            order.OrderDate = DateTime.Now;
            order.Status = OrderStatus.Pending;

            // Recorrer detalles para verificar menús y calcular totales
            foreach (var detail in order.OrderDetails)
            {
                var menu = await _menuRepository.GetByIdAsync(detail.MenuId);
                if (menu == null)
                    throw new KeyNotFoundException($"Menu with ID {detail.MenuId} not found");

                if (!menu.IsActive)
                    throw new InvalidOperationException($"Menu with ID {detail.MenuId} is not active");

                if (menu.Stock < detail.Quantity)
                    throw new InvalidOperationException($"Insufficient stock for menu with ID {detail.MenuId}");

                // Establecer precios
                detail.UnitPrice = menu.Price;
                detail.Subtotal = menu.Price * detail.Quantity;

                // Actualizar stock del menú
                menu.Stock -= detail.Quantity;
                await _menuRepository.UpdateAsync(menu);
            }

            // Calcular total del pedido
            order.Total = order.OrderDetails.Sum(d => d.Subtotal);

            await _orderRepository.AddAsync(order);
            return order;
        }

        public async Task UpdateOrderAsync(Order order)
        {
            if (order == null)
                throw new ArgumentNullException(nameof(order));

            // Verificar que el pedido existe
            var existingOrder = await _orderRepository.GetByIdAsync(order.Id);
            if (existingOrder == null)
                throw new KeyNotFoundException($"Order with ID {order.Id} not found");

            // No permitir cambios si el pedido ya está entregado o cancelado
            if (existingOrder.Status == OrderStatus.Delivered || existingOrder.Status == OrderStatus.Cancelled)
                throw new InvalidOperationException($"Cannot update order with ID {order.Id} because it is already {existingOrder.Status}");

            await _orderRepository.UpdateAsync(order);
        }

        public async Task DeleteOrderAsync(int id)
        {
            var order = await _orderRepository.GetByIdAsync(id);
            if (order == null)
                throw new KeyNotFoundException($"Order with ID {id} not found");

            // Solo permitir eliminar pedidos pendientes
            if (order.Status != OrderStatus.Pending)
                throw new InvalidOperationException($"Cannot delete order with ID {id} because it is not pending");

            await _orderRepository.DeleteAsync(id);
        }

        public async Task UpdateOrderStatusAsync(int orderId, OrderStatus status)
        {
            var order = await _orderRepository.GetByIdAsync(orderId);
            if (order == null)
                throw new KeyNotFoundException($"Order with ID {orderId} not found");

            // Validar transiciones de estado
            if (order.Status == OrderStatus.Cancelled)
                throw new InvalidOperationException("Cannot update status of a cancelled order");

            if (order.Status == OrderStatus.Delivered && status != OrderStatus.Delivered)
                throw new InvalidOperationException("Cannot change status of a delivered order");

            // Validar secuencia lógica de estados
            if (status < order.Status && status != OrderStatus.Cancelled)
                throw new InvalidOperationException($"Cannot change order status from {order.Status} to {status}");

            await _orderRepository.UpdateOrderStatusAsync(orderId, status);
        }

        public async Task<decimal> CalculateOrderTotalAsync(int orderId)
        {
            var order = await _orderRepository.GetOrderWithDetailsAsync(orderId);
            if (order == null)
                throw new KeyNotFoundException($"Order with ID {orderId} not found");

            return order.OrderDetails.Sum(d => d.Subtotal);
        }
    }
}