using Library_Management_System.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Library_Management_System.Web.Models.ApiResponse
{
    public class RoleResponse
    {
        public int RoleId { get; set; }
        public Roles Role { get; set; }
    }
}
