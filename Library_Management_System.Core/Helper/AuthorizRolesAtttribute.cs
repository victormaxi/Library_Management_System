//using Library_Management_System.Core.Interfaces;
//using Library_Management_System.Core.Models;
//using System;
//using System.Collections.Generic;
//using System.Text;

//namespace Library_Management_System.Core.Helper
//{
//    public class AuthorizRolesAtttribute : AuthorizeAttribute
//    {
//        private Roles[] _roles;
//        private readonly IAccountServices _account;

//        public AuthorizRolesAtttribute(IAccountServices account,params Roles[] roles)
//        {
//            _roles = roles;
//            _account = account;
//        }
        
//        //protected override bool IsAuthorized(HttpActionContext actionContext )
//        //{
//        //    return _roles.Contains(Globals.CurrentUser.Role);
//        //}

       
//    }
//}
