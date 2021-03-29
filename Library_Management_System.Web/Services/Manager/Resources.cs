using Library_Management_System.Core.Models;
using Library_Management_System.Core.ViewModels;
using Library_Management_System.Web.Helper;
using Library_Management_System.Web.Models.ApiResponse;
using Library_Management_System.Web.Services.Intrerface;
using Library_Management_System_Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Library_Management_System.Web.Services.Manager
{
    public class Resources : IResources
    {
        private readonly ApiRequestUri _apiRequestUri;
        private readonly ApplicationDBContext _dataContext;
        private readonly IHostingEnvironment _env;

        public Resources(IOptionsSnapshot<ApiRequestUri> options, ApplicationDBContext dataContext, IHostingEnvironment env)
        {
            _apiRequestUri = options.Value;
            _dataContext = dataContext;
            _env = env;
        }

        public async Task<IEnumerable<BookVM>> GetBooks()
        {
            //try
            //{
            //    var allBooks = await _dataContext.Books.ToListAsync();
            //    var BookVM = new BookVM()
            //    {
            //        BookCode = allBooks
            //    }
            //    return allBooks;
            //}
            //catch(Exception ex)
            //{
            //    throw new Exception(ex.Message);
            //}

            try
            {
                using (HttpClient client = new HttpClient())
                {
                    client.BaseAddress = new Uri(_apiRequestUri.BaseUri);
                    var responeState = await client.GetAsync(_apiRequestUri.GetBooks);
                    if (responeState.IsSuccessStatusCode)
                    {
                        var taskCR = responeState.Content.ReadAsStringAsync();
                        var responseStringCR = taskCR.Result;
                        var BKResource = JsonConvert.DeserializeObject<List<BookVM>>(responseStringCR);
                        return BKResource.ToList();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return null;
        }

        public ApplicationUser GetUserDetails(string id)
        {
            var details = (from p in _dataContext.Users where p.Id == id select p).SingleOrDefault();
            return details;
        }

      


        public async Task<RoleResponse> GetUserRoles(int Id)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    client.BaseAddress = new Uri(_apiRequestUri.BaseUri);
                    var uri = string.Format(_apiRequestUri.GetUserRoles, Id);
                    var response = await client.GetAsync(uri);
                    if (response.IsSuccessStatusCode)
                    {
                        var task = response.Content.ReadAsStringAsync();
                        var result = task.Result;
                        var roles = JsonConvert.DeserializeObject<RoleResponse>(result);
                        return roles;
                    }
                    return null;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.InnerException.ToString());
            }
        }

     
    }
}
