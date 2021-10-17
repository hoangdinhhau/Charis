using System.Collections.Generic;

namespace Charis.Data.Entities
{
    public class Role
    {
        public int Id { get; set; }
        public string RoleName { get; set; }
        public bool SoftDelete { get; set; }
        public List<User> Users { get; set; }
    }
}