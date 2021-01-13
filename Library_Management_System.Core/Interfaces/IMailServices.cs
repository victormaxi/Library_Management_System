using Library_Management_System.Core.Models;
using Library_Management_System.Core.Utility;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Library_Management_System.Core.Interfaces
{
    public interface IMailServices
    {
        Task SendEmailAsync(string toEmail, string subject, string content);

        //void SendEmail(EmailMessage emailMessage);
        void SendEmail2(string userEmail, string url);
        //void SendEmail2(EmailMessage emailMessage, string url);
        Task<bool> SendMail(string email, string subject, string body, EmailConfig emailConfig);
        //Task SendMail(string email, string subject, (ApplicationUser user, string callbackUrl) body, EmailConfig emailConfig);

        void Send(string to, string subject, string html, string from = null);
    }
}
