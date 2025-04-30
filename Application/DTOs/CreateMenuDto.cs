using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class CreateMenuDto
    {
        public string Name { get; set; }
        public int Number { get; set; }
        public decimal Price { get; set; }
        public bool IsActive { get; set; }
        public int Stock { get; set; }
        public string Description { get; set; }
        public DateTime ValidFrom { get; set; }
        public DateTime? ValidTo { get; set; }
    }
}