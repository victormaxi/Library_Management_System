using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Library_Management_System.Core.Interfaces;
using Library_Management_System.Core.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Library_Management_System.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountServices _accountServices;
        public AccountController(IAccountServices accountServices)
        {
            _accountServices = accountServices;
        }
        [HttpPost]
        [Route("loginUserAsync")]
        public async Task<IActionResult> LoginUserAsync ([FromBody]LoginVM loginVm)
        {
            try
            {
                var exist = await _accountServices.IsExist(loginVm.UserName);
                if(exist!= null)
                {
                    var result = await _accountServices.LoginUser(loginVm);
                    if(result.IsSuccess)
                    {
                        return Ok(result);
                    }
                    return BadRequest(result);

                }
                return BadRequest("Some information are invalid");
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [HttpPost]
        [Route("registerUserAsync")]
        public async Task<IActionResult> RegisterUserAsync ([FromBody]ApplicationUserVM userVM)
        {
            try
            {
                var exist = await _accountServices.IsExist(userVM.UserName);
                if(!exist.IsSuccess)
                {
                   var result = await _accountServices.RegisterAccountAsync(userVM);
                    if(result.IsSuccess)
                    {
                        return Ok(result);
                    }
                    return BadRequest(result);
                }
                else
                {
                    return BadRequest("Invalid Details");
                }
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
