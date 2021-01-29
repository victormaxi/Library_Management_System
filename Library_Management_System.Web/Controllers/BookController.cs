using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Library_Management_System.Core.ViewModels;
using Library_Management_System.Web.Helper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace Library_Management_System.Web.Controllers
{
    public class BookController : BaseController
    {
        private readonly ApiRequestUri _apiRequestUri;
        private readonly IHttpContextAccessor _HttpContext;



        public BookController(IOptionsSnapshot<ApiRequestUri> options, IHttpContextAccessor _httpContext) : base(_httpContext.HttpContext)
        {
            _apiRequestUri = options.Value;
            _HttpContext = _httpContext;

        }
        [Authorize]
        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> GetBooks()
        {
            try
            {
                 BookVM bookVM = new BookVM();
                if (this.ModelState.IsValid)
                    using (var httpClient = new HttpClient())
                    {
                        httpClient.BaseAddress = new Uri(_apiRequestUri.BaseUri);
                        httpClient.DefaultRequestHeaders.Accept.Clear();
                        httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                        var uri = string.Format(_apiRequestUri.GetBooks);

                        HttpResponseMessage res = await httpClient.GetAsync(uri);

                        if (res.IsSuccessStatusCode)
                        {
                            var apiTask = res.Content.ReadAsStringAsync();
                            var responseString = apiTask.Result;
                            var model = JsonConvert.DeserializeObject<BookVMList>(responseString);


                            //var model = JsonConvert.DeserializeObject<StudentDto>(responseString); 
                            ////to save info in session
                            //HttpContext.Session.SetString("StudentId", model.StudentId.ToString());

                            //to get saved info from session
                            //string studentId = HttpContext.Session.GetString("StudentId");

                            return View(model);
                        }
                    }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return View();
        }
    }
}
