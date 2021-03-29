
using Library_Management_System.Core.Models;
using Library_Management_System.Web.Helper;
using Library_Management_System.Web.Models;
using Library_Management_System.Web.Services.Intrerface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Diagnostics;

namespace Library_Management_System.Web.Controllers
{
   
    [Authorize(Roles = Role.Admin)]
    public class HomeController : BaseController
    {
        private readonly ILogger<HomeController> _logger;

        private readonly IHttpContextAccessor _HttpContext;
        private readonly ApiRequestUri _apiRequestUri;
        private readonly IResources _resource;
        private readonly IHostingEnvironment _appEnvironment;

        public HomeController(IOptionsSnapshot<ApiRequestUri> options, IResources resource, IHostingEnvironment appEnvironment, IHttpContextAccessor _httpContext) : base(_httpContext.HttpContext)
        {
            _apiRequestUri = options.Value;
            _resource = resource;
            _appEnvironment = appEnvironment;
            _HttpContext = _httpContext;
        }

        public IActionResult Index()
        {
            var UserId = GetRoles();
            var UserName = GetEmail();
            ViewBag.Id = UserId;
            ViewBag.UserName = UserName;
            return View();
        }
        public IActionResult UnAuthorized()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
