using System;

namespace Charis.Data.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public DateTime BirthDay { get; set; }
        public string Address { get; set; }
        public string PassWord { get; set; }
        public string Email { get; set; }
        public bool SoftDelete { get; set; }
        public int RoleId { get; set; }
        public Role Role { get; set; }
    }
}