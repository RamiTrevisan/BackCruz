

// Domain/Entities/User.cs
using System;
using System.Collections.Generic;
using System.Data;

namespace Domain.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public int RoleId { get; set; }
        public Role Role { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? LastLogin { get; set; }
    }
}

#region viejo
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace Domain.Entities
//{
//    public class User
//    {
//        public int Id { get; set; }
//        public string Name { get; set; }
//        public string LastName { get; set; }
//        public string Email { get; set; }
//        public string Password { get; set; }
//        public string UserRol { get; set; }
//    }
//}
#endregion