using System.Collections.Generic;
namespace Domain.Entities
{
    public class Customer
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public User User { get; set; }
        public int? UserId { get; set; }
        public ICollection<Order> Orders { get; set; }
    }
}