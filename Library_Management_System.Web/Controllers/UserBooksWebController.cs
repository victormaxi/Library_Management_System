using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Library_Management_System.Core.Models;
using Library_Management_System.Core.ViewModels;
using Library_Management_System.Web.Helper;
using Library_Management_System.Web.Services.Intrerface;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace Library_Management_System.Web.Controllers
{
    public class UserBooksWebController : BaseController
    {
        private readonly ApiRequestUri _apiRequestUri;
        private readonly IHttpContextAccessor _HttpContext;
        private readonly IResources _resource;
        private readonly IHostingEnvironment _appEnvironment;



        public UserBooksWebController(IOptionsSnapshot<ApiRequestUri> options, IHttpContextAccessor _httpContext, IResources resource, IHostingEnvironment appEnvironment) : base(_httpContext.HttpContext)
        {
            _resource = resource;
            _appEnvironment = appEnvironment;
            _apiRequestUri = options.Value;
            _HttpContext = _httpContext;

        }
        [HttpGet]
        public async  Task<IActionResult> Index()
        {
            var listOfBooks = await  _resource.GetBooks();

            ViewData["newBook"] = listOfBooks;

            ViewBag.BookLists = new SelectList(listOfBooks, "BookId", "Name");

            ViewBag.message = listOfBooks;
            return View();
        }

        public async Task<IActionResult> GetBooksForUser(string userId)
        {

            try
            {
                userId = GetUserId();
                if (this.ModelState.IsValid)
                    using (var httpClient = new HttpClient())
                    {

                        httpClient.BaseAddress = new Uri(_apiRequestUri.BaseUri);
                        httpClient.DefaultRequestHeaders.Accept.Clear();
                        httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer",
                            HttpContext.Session.GetString("token"));

                       // var uri = _apiRequestUri.BaseUri + _apiRequestUri.GetBooksForUser + id;

                        var uri = string.Format(_apiRequestUri.GetBooksForUser, userId);
                        HttpResponseMessage res = await httpClient.GetAsync(uri);

                        if (res.IsSuccessStatusCode)
                        {
                            var apiTask = res.Content.ReadAsStringAsync();
                            var responseString = apiTask.Result;
                            var model = JsonConvert.DeserializeObject<List<BookVM>>(responseString);
                            if (res.StatusCode == HttpStatusCode.Unauthorized)
                            {
                                ViewBag.Message = "Unauthorized!";
                            }
                            else
                            {
                                return View(model);
                            }
                        }
                    }
                return View();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
     }
}
