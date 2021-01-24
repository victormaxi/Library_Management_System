using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Library_Management_System.Core.Interfaces;
using Library_Management_System.Core.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace Library_Management_System.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountServices _accountServices;
        private IConfiguration _configuration;
        public AccountController(IAccountServices accountServices, IConfiguration configuration)
        {
            _accountServices = accountServices;
            _configuration = configuration;
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

       [HttpGet]
       [Route("ConfirmEmail/{userId}/{token}")]
        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            try
            {
                if (string.IsNullOrEmpty(userId) || string.IsNullOrWhiteSpace(token))
                    return NotFound();

                var result = await _accountServices.ConfirmEmailAsync(userId,token);

                if(result.IsSuccess)
                {
                    return Redirect($"{_configuration["AppUrl"]}/ConfirmEmail.html");
                }
                return BadRequest(result);
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [HttpPost]
        [Route("ResetPassword")]
        public async Task<IActionResult> RestPasswordAsync (string email)
        {
            try
            {
                if (string.IsNullOrEmpty(email))
                    return NotFound();

                var result = await _accountServices.ForgotPasswordAsync(email);
                if (result.IsSuccess)
                    return Ok(result);//200

                return BadRequest(result); //400
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
