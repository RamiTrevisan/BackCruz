﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class CreateOrderDetailDto
    {
        public int MenuId { get; set; }
        public int Quantity { get; set; }
    }
}
