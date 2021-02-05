using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
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
                        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer",
                            HttpContext.Session.GetString("token"));

                        var uri = string.Format(_apiRequestUri.GetBooks);

                        HttpResponseMessage res = await httpClient.GetAsync(uri);

                        if (res.IsSuccessStatusCode)
                        {
                            var apiTask = res.Content.ReadAsStringAsync();
                            var responseString = apiTask.Result;
                            var model = JsonConvert.DeserializeObject<BookVMList>(responseString);
                            if (res.StatusCode == HttpStatusCode.Unauthorized)
                            {
                                ViewBag.Message = "Unauthorized!";
                            }
                            else
                            {
                                return View(model);
                            }

                                //var model = JsonConvert.DeserializeObject<StudentDto>(responseString); 
                                ////to save info in session
                                //HttpContext.Session.SetString("StudentId", model.StudentId.ToString());

                                //to get saved info from session
                                //string studentId = HttpContext.Session.GetString("StudentId");

                               
                        }
                    }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return View();
        }

        [HttpGet]
        public ActionResult AddBook()
        {
            
            return View();
        }
        [HttpPost]
        public async Task<ActionResult> AddBook(AddBookVM bookVM)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(bookVM);
                }

                using (var httpClient = new HttpClient())
                {
                    httpClient.BaseAddress = new Uri(_apiRequestUri.BaseUri);
                    httpClient.DefaultRequestHeaders.Accept.Clear();
                    httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    var book = new AddBookVM()
                    {
                        BookCode = bookVM.BookCode, 
                        BookTitle = bookVM.BookTitle
                    };

                    var uri = string.Format(_apiRequestUri.AddBook, book);

                    StringContent content = new StringContent(JsonConvert.SerializeObject(bookVM));

                    //HttpResponseMessage response = await httpClient.PostAsync(uri, content);
                    HttpResponseMessage response = (HttpResponseMessage)null;

                    response = await httpClient.PostAsJsonAsync(uri, bookVM);
                    if (response.IsSuccessStatusCode)
                    {

                        return RedirectToAction("GetBooks");
                        //return RedirectToAction("GetStudentById", new { id = student.ID });
                    }
                    if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                    {
                        ModelState.AddModelError("", await response.Content.ReadAsStringAsync());
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return View();
        }


//        string baseUrl = "http://localhost:49387";
//        HttpClient client = new HttpClient();
//        client.BaseAddress = new Uri(baseUrl);
//        var contentType = new MediaTypeWithQualityHeaderValue
//    ("application/json");
//        client.DefaultRequestHeaders.Accept.Add(contentType);

//    client.DefaultRequestHeaders.Authorization =
//new AuthenticationHeaderValue("Bearer",
//HttpContext.Session.GetString("token"));

//    HttpResponseMessage response = client.GetAsync
//("/api/employee").Result;
//        string stringData = response.Content.
//    ReadAsStringAsync().Result;
//        List<Employee> data = JsonConvert.DeserializeObject
//    <List<Employee>>(stringData);

//    if (response.StatusCode == HttpStatusCode.Unauthorized)
//    {
//        ViewBag.Message = "Unauthorized!";
//    }
//    else
//    {
//        string strTable = "<table border='1' cellpadding='10'>";
//        foreach (Employee emp in data)
//        {
//            strTable += "<tr>";
//            strTable += "<td>";
//            strTable += emp.EmployeeID;
//            strTable += "</td>";
//            strTable += "<td>";
//            strTable += emp.FirstName;
//            strTable += "</td>";
//            strTable += "<td>";
//            strTable += emp.LastName;
//            strTable += "</td>";
//            strTable += "</tr>";

//        }
//strTable += "</table>";

//        ViewBag.Message = strTable;
    }
}

