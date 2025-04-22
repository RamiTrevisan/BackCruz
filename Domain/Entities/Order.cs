using System;
using System.Collections.Generic;

namespace Domain.Entities
{
    public class Order
    {
        public int Id { get; set; }
        public DateTime OrderDate { get; set; }
        public int CustomerId { get; set; }
        public Customer Customer { get; set; }
        public OrderStatus Status { get; set; }
        public decimal Total { get; set; }
        public string DeliveryAddress { get; set; }
        public string Notes { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
        public bool IsPaid { get; set; }
        public ICollection<OrderDetail> OrderDetails { get; set; }
    }
}