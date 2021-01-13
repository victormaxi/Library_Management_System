using Library_Management_System.Core.Extensions;
using Library_Management_System.Core.Helper;
using Library_Management_System.Core.Interfaces;
using Library_Management_System.Core.Models;
using Library_Management_System.Core.ViewModels;
using Library_Management_System.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Library_Management_System.Domain.Services
{
    public class AccountServicesManager : IAccountServices
    {
        private readonly UserManager<ApplicationUser> userManager;

        private readonly ApplicationDBContext context;
        private readonly IMailServices _mailServices;
        private IConfiguration _configuration;


        public AccountServicesManager(UserManager<ApplicationUser> userManager, ApplicationDBContext context, IMailServices mailServices, IConfiguration configuration)
        {
            this.userManager = userManager;

            this.context = context;

            _mailServices = mailServices;

            _configuration = configuration;

        }

        public async Task<UserManagerResponse> ConfirmEmailAsync(string userId, string token)
        {
            try
            {
                var user = await userManager.FindByIdAsync(userId);

                if (user == null)
                {
                    return new UserManagerResponse
                    {
                        Message = "User not found",
                        IsSuccess = false
                    };
                }

                var decodedToken = WebEncoders.Base64UrlDecode(token);
                string normalToken = Encoding.UTF8.GetString(decodedToken);

                var result = await userManager.ConfirmEmailAsync(user, normalToken);

                if (result.Succeeded)
                {
                    return new UserManagerResponse
                    {
                        Message = "Email confirmed successfully",
                        IsSuccess = true
                    };
                }
                else
                {
                    return new UserManagerResponse
                    {
                        Message = "Email did not confirm",
                        IsSuccess = false,
                        Errors = result.Errors.Select(e => e.Description)
                    };
                };
            }
            
    
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<UserManagerResponse> ForgotPasswordAsync(string email)
        {
            try
            {
                var user = await userManager.FindByEmailAsync(email);

                if(user == null)
                
                    return new UserManagerResponse
                    {
                        Message = "No user associated with this email.",
                        IsSuccess = false
                    };

                    var token = await userManager.GeneratePasswordResetTokenAsync(user);
                    var encodedToken = Encoding.UTF8.GetBytes(token);
                    var validToken = WebEncoders.Base64UrlEncode(encodedToken);

                    string url = $"{_configuration["AppUrl"]}/ResetPassword?email={email}&tokn={validToken}";
                    await _mailServices.SendEmailAsync(email, "Reset Password", "<h1>Follow the instructions to reset your password</h1>"+$"<p>To reset your password <a href='{url}'>Click here</a></p>");


                return new UserManagerResponse
                {
                    Message = "Reset Url has been sent to your email address",
                    IsSuccess = true
                };
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
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
                if (userVM == null)
                {
                    throw new NullReferenceException("Register model is null");
                }
                if (userVM.Password != userVM.ConfirmPassword)
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

                    var confirmEmailToken = await userManager.GenerateEmailConfirmationTokenAsync(user);
                    // var confirmationLink = Url.Action("ConfirmEmail", "Email", new { token, email = user.Email }, Request.Scheme);
                    var encodedEmailToken = Encoding.UTF8.GetBytes(confirmEmailToken);
                    var validEmailToken = WebEncoders.Base64UrlEncode(encodedEmailToken);
                   // var url = $"{_configuration["AppUrl"]}/api/account/confirmmailasync?userid={user.Id}&token={validEmailToken}" ;
                  //  string link = "Confirm your account Welcome to Library management system" + $"<p>Please confirm your email by <a href='{url}'>clicking here</a></p>";
                    //await _mailServices.SendMail(user.Email, "Confirm your account", $"<h1>Welcome to Library management system</h1>" + $"<p>Please confirm your email by <a href='{url}'>clicking here</a></p>");


                   // string code = HttpUtility.UrlEncode(await userManager.GenerateEmailConfirmationTokenAsync(userEmail));

                    var callbackUrl = $"{_configuration["AppUrl"]}/api/account/ConfirmEmail?userid={user.Id}&token={validEmailToken}";
                   // var body = Util.PrepareRegistrationEmailTemplate(user, callbackUrl);
                   //  var body = ""+(user, callbackUrl);

                   // var subject = "Email Confirmation";

                    //_logger.LogInformation($"Sending registration email to {user.Email} ");

                    // await _mailServices.Send(user.Email, subject, body);
               //     _mailServices.Send(
               //to: user.Email,
               //subject: "Sign-up Verification API - Verify Email",
               //html: $@"<h4>Verify Email</h4>
               //          <p>Thanks for registering!</p>
               //          {callbackUrl}");

                    _mailServices.SendEmail2(user.Email, $"<a href='{callbackUrl}'> Click here</a>");
                    //EmailHelper emailHelper = new EmailHelper();
                    //bool emailResponse = emailHelper.SendEmail(user.Email, callbackUrl);


                    // var confirmEmailToken = await userManager.GenerateEmailConfirmationTokenAsync(user);
                    // var encodedEmailToken = Encoding.UTF8.GetBytes(confirmEmailToken);
                    //var validEmaillToken = WebEncoders.Base64UrlEncode(encodedEmailToken);

                    //string url = $"{_configuration["AppUrl"]}/api/account/confirmrmail?userid={user.Id}&token={validEmaillToken}";

                    //await _mailServices.SendEmailAsync(user.Email, "Confirm your account", $"<h1>Welcome to Library management system</h1>" + $"<p>Please confirm your email by <a href='{url}'>clicking here</a></p>");

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

        public Task SendConfirmRegistrationMail(string userEmail, string WebUrl)
        {
            throw new NotImplementedException();
        }
        //public async Task SendConfirmRegistrationMail(string userEmail, string WebUrl)
        //{
        //   try
        //    {
        //        //var user = GetBaseUser(a => a.Email == userEmail);

        //        string code = HttpUtility.UrlEncode(await userManager.GenerateEmailConfirmationTokenAsync(userEmail));

        //        var callbackUrl = $"{WebUrl}/Account/ConfirmEmail?userId={user.Id}&code={code}";
        //        var body = Util.PrepareRegistrationEmailTemplate(user, callbackUrl);

        //        var subject = "Email Confirmation";

        //        _logger.LogInformation($"Sending registration email to {user.Email} ");

        //        await emailService.SendMail(user.Email, subject, body, configuration.GetEmailConfig());

        //        logger.LogInformation($"successfully sent registration email to {user.Email} ");

        //        }
        //        catch (Exception ex)
        //        {
        //            logger.LogError(ex.Message);
        //        }
        //    }

    }
}
