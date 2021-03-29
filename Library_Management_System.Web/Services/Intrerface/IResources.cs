using Library_Management_System.Core.Models;
using Library_Management_System.Core.ViewModels;
using Library_Management_System.Web.Models.ApiResponse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Library_Management_System.Web.Services.Intrerface
{
    public interface IResources
    {
        ApplicationUser GetUserDetails(string id);
        Task<RoleResponse> GetUserRoles(int Id);

        Task<IEnumerable<BookVM>> GetBooks();
    }
}
