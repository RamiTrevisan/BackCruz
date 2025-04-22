using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class MenuFoodItem
    {
        public int Id { get; set; }
        public int MenuId { get; set; }
        public Menu Menu { get; set; }
        public int FoodItemId { get; set; }
        public FoodItem FoodItem { get; set; }
        public int Quantity { get; set; }
    }
}