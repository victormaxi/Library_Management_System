using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Library_Management_System.Core.Models
{
    public class ApplicationUser : IdentityUser
    {
        public int CardId { get; set; }
        //public Roles Role { get; set; }
        public string Role { get; set; }
        public virtual List<UserBooks> UserBooks { get; set; }
    }
}
