using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Library_Management_System.Web.Helper
{
    public class ApiRequestUri
    {
        public string BaseUri { get; set; }
        public string Register { get; set; }
        public string Login { get; set; }
        public string ConfirmEmail { get; set; }
        public string ForgotPassword { get; set; }


        //Book Uri
        public string GetBooks { get; set; }
        public string AddBook { get; set; }
        public string GetUserRoles { get; set; }


        public string SelectCourse { get; set; }
        public string AddUserBooks { get; set; }
        public string GetBooksForUser { get; set; }
    }
}
