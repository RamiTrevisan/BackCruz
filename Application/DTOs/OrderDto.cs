using Domain.Entities;
using System;
using System.Collections.Generic;

namespace Application.DTOs
{
    public class OrderDto
    {
        public int Id { get; set; }
        public DateTime OrderDate { get; set; }
        public int CustomerId { get; set; }
        public string CustomerName { get; set; }
        public OrderStatus Status { get; set; }
        public string StatusName { get; set; }
        public decimal Total { get; set; }
        public string DeliveryAddress { get; set; }
        public string Notes { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
        public string PaymentMethodName { get; set; }
        public bool IsPaid { get; set; }
        public ICollection<OrderDetailDto> OrderDetails { get; set; }
    }
}