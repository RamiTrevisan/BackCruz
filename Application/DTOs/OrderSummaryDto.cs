using Domain.Entities;
using System;

namespace Application.DTOs
{
    public class OrderSummaryDto
    {
        public int Id { get; set; }
        public DateTime OrderDate { get; set; }
        public OrderStatus Status { get; set; }
        public string StatusName { get; set; }
        public decimal Total { get; set; }
        public int ItemCount { get; set; }
    }
}