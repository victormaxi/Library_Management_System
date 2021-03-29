//using Library_Management_System.Core.Models;
//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.AspNetCore.Mvc.Filters;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Security.Claims;
//using System.Text;
////using System.Web.Mvc;

//namespace Library_Management_System.Core.Helper
//{
//    //[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
//    //public class AuthorizeAttribute : Attribute, IAuthorizationFilter
//    //{
//    //    private readonly IList<Roles> _roles;

//    //    public AuthorizeAttribute(params Roles[] roles)
//    //    {
//    //        _roles = roles ?? new Roles[] { };
//    //    }

//    //    public void OnAuthorization(AuthorizationFilterContext context)
//    //    {
           
//    //        //var account = (ApplicationUser)context.HttpContext.Items["ApplicationUser"];
//    //        //if (account == null || (_roles.Any() && !_roles.Contains(account.Role)))
//    //        //{
//    //        //    // not logged in or role not authorized
//    //        //    context.Result = new JsonResult(new { message = "Unauthorized" }) { StatusCode = StatusCodes.Status401Unauthorized };
//    //        //}
//    //    }


//    //    //public List<string> GetRoles()
//    //    //{
//    //    //    var claimsIdentity = context.User.Identity as ClaimsIdentity;
//    //    //    if (claimsIdentity.IsAuthenticated == false)
//    //    //        return null;
//    //    //    var roles = claimsIdentity.Claims;
//    //    //    List<string> rolesLst = new List<string>();
//    //    //    foreach (var claim in roles)
//    //    //    {
//    //    //        rolesLst.Add(claim.Value);
//    //    //    }
//    //    //    return rolesLst;
//    //    //}
//    //}

//}
