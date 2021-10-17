﻿using System;

namespace Charis.ModelView.Catalog.UserModel
{
    public class UserViewModel
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public DateTime BirthDay { get; set; }
        public string Address { get; set; }
        public string PassWord { get; set; }
        public string Email { get; set; }
        public bool SoftDelete { get; set; }
        public string RoleName { get; set; }
    }
}