using Library_Management_System.Core.Models;
using System.Collections.Generic;
using System.Linq;

namespace Library_Management_System.Core.Helper
{
    public  static class ExtensionMethods
    {
        public static IEnumerable<ApplicationUser> WithoutPassword (this IEnumerable<ApplicationUser> users)
        {
            if (users == null) 
            return null;

            return users.Select(x => x.WithoutPassword());
        }

        public static ApplicationUser WithoutPassword(this ApplicationUser user)
        {
            if (user == null)
             return null;

             user.PasswordHash = null;
             return user;
        }
    }
}