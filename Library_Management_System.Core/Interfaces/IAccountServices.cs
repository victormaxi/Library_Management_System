using Library_Management_System.Core.Models;
using Library_Management_System.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Library_Management_System.Core.Interfaces
{
    public interface IAccountServices
    {
        Task<UserManagerResponse> IsExist(string userName);
        Task <UserManagerResponse> RegisterAccountAsync(ApplicationUserVM userVM);
        Task<UserManagerResponse> LoginUser(LoginVM loginVM);
        Task<UserManagerResponse> ConfirmEmailAsync(string userId, string token);
        Task<UserManagerResponse> ForgotPasswordAsync(string email);
        Task SendConfirmRegistrationMail(string userEmail, string WebUrl);
    }
}
