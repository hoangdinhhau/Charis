using System;

namespace Charis.Charis.ModelView.Catalog.UserModel
{
    public class UserUpdateRequest
    {
        public string FullName { get; set; }
        public DateTime BirthDay { get; set; }
        public string Address { get; set; }
        public string PassWord { get; set; }
        public string Email { get; set; }
        public int RoleId { get; set; }
    }
}