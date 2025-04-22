using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Domain.Entities;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class CreateOrderDto
    {
        public int CustomerId { get; set; }
        public string DeliveryAddress { get; set; }
        public string Notes { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
        public List<CreateOrderDetailDto> OrderDetails { get; set; }
    }
}
