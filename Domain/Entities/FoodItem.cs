using System.Collections.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class FoodItem
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int CategoryId { get; set; }
        public FoodCategory Category { get; set; }
        public bool IsAvailable { get; set; }
        public int Stock { get; set; }
        public string ImageUrl { get; set; }
        public ICollection<MenuFoodItem> MenuItems { get; set; }
    }
}