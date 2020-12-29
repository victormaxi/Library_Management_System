using Library_Management_System.Core.Interfaces;
using Library_Management_System.Core.Models;
using Library_Management_System.Core.ViewModels;
using Library_Management_System.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Library_Management_System.Domain.Services
{
    public class AccountServicesManager : IAccountServices
    {
        private readonly UserManager<ApplicationUser> userManager;
        
        private readonly ApplicationDBContext context;

        private IConfiguration _configuration;

        public AccountServicesManager(UserManager<ApplicationUser> userManager, ApplicationDBContext context, IConfiguration configuration)
        {
            this.userManager = userManager;
            
            this.context = context;

            _configuration = configuration;
        }

        public async Task<UserManagerResponse> IsExist(string userName)
        {
            try
            {
                var userExist = await userManager.FindByNameAsync(userName);
                if(userExist != null)
                {
                    return new UserManagerResponse
                    {
                        Message = "User name already exist",
                        IsSuccess = false,
                        
                    };
                }
                
                return new UserManagerResponse()
                {
                    Message = ""
                };
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<UserManagerResponse> LoginUser(LoginVM loginVM)
        {
            try
            {
                var userExist = await userManager.FindByNameAsync(loginVM.UserName);
                if(userExist == null)
                {
                    return new UserManagerResponse
                    {
                        Message = "There is no user found",
                        IsSuccess = false
                    };
                }
                var result = await userManager.CheckPasswordAsync(userExist, loginVM.Password);
                if(!result)
                {
                    return new UserManagerResponse
                    {
                        Message = "Invalid Password",
                        IsSuccess = false
                    };
                }
                var claims = new[]
                {
                    new Claim ("UserName",loginVM.UserName),
                    new Claim(ClaimTypes.NameIdentifier,userExist.Id)
                };

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["AuthSettings:key"]));

                var token = new JwtSecurityToken(
                    issuer: _configuration["AuthSettings:Issuer"],
                    audience: _configuration["AuthSettings:Audience"],
                    claims: claims,
                    expires: DateTime.Now.AddDays(30),
                    signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256));

                string tokenAsString = new JwtSecurityTokenHandler().WriteToken(token);

                return new UserManagerResponse
                {
                    Message = tokenAsString,
                    IsSuccess = true,
                    ExpireDate = token.ValidTo
                };
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
           
        }

        public async Task<UserManagerResponse> RegisterAccountAsync(ApplicationUserVM userVM)
        {
            try
            {
                if(userVM == null)
                {
                    throw new NullReferenceException("Register model is null");
                }
                if(userVM.Password != userVM.ConfirmPassword)
                {
                    return new UserManagerResponse
                    {
                       Message = "Confirm password doesn't match password",
                       IsSuccess = false
                    };
                }
                    var user = new ApplicationUser()
                    {
                        UserName = userVM.UserName,
                        PhoneNumber = userVM.PhoneNumber,
                        Email = userVM.Email,

                    };
                    var result = await userManager.CreateAsync(user, userVM.Password);
                    if (result.Succeeded)
                    {
                        return new UserManagerResponse
                        { 
                        Message = "User created successfully!!",
                        IsSuccess = true
                        };
                    }


                return new UserManagerResponse
                {
                    Message = "User not created",
                    IsSuccess = false,
                    Errors = result.Errors.Select(e => e.Description)
                };
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
