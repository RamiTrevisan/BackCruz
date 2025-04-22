using System;

using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class MenuFoodItemDto
    {
        public int Id { get; set; }
        public int MenuId { get; set; }
        public int FoodItemId { get; set; }
        public string FoodItemName { get; set; }
        public decimal FoodItemPrice { get; set; }
        public int Quantity { get; set; }
    }
}
