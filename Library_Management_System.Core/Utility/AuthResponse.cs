using Library_Management_System.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Library_Management_System.Core.Utility
{
   public class AuthResponse
    {

        public string userId { get; set; }
        public string username { get; set; }
        public string Email { get; set; }
        public string token { get; set; }
        public ICollection<Roles> Roles { get; set; }
        public DateTime expiryDate { get; set; }

    }
}
