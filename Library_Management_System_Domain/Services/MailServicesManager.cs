using Library_Management_System.Core.Interfaces;
using Library_Management_System.Core.Models;
using Microsoft.Extensions.Configuration;


using MailKit.Net.Smtp;
using System;

using System.Threading.Tasks;
using System.Net.Mail;
using Library_Management_System.Core.Utility;


using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MimeKit.Text;
using MailKit.Security;
using MimeKit;

namespace Library_Management_System.Domain.Services
{
    public class MailServicesManager : IMailServices
    {
        private IConfiguration _configuration;
        private readonly EmailConfiguration _emailConfiguration;
        private readonly ILogger<MailServicesManager> _logger;

        //public EmailService(IOptions<AppSettings> appSettings)
        //{
        //    _appSettings = appSettings.Value;
        //}

        public MailServicesManager(IConfiguration configuration, IOptions<EmailConfiguration> emailConfiguration, ILogger<MailServicesManager> logger)
        {
            _logger = logger;
            _configuration = configuration;
            _emailConfiguration = emailConfiguration.Value;
        }

        public Task SendEmailAsync(string toEmail, string subject, string content)
        {
            throw new NotImplementedException();
        }

        public Task<bool> SendMail(string email, string subject, string body, EmailConfig emailConfig)
        {
            throw new NotImplementedException();
        }

        public void Send(string to, string subject, string html, string from = null)
        {
            // create message
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse(from ?? _emailConfiguration.EmailFrom));
            email.To.Add(MailboxAddress.Parse(to));
            email.Subject = subject;
            email.Body = new TextPart(TextFormat.Html) { Text = html };

            // send email
            using var smtp = new MailKit.Net.Smtp.SmtpClient();
            smtp.Connect(_emailConfiguration.SmtpHost, _emailConfiguration.SmtpPort, SecureSocketOptions.StartTls);
            smtp.Authenticate(_emailConfiguration.SmtpUser, _emailConfiguration.SmtpPass);
            smtp.Send(email);
            smtp.Disconnect(true);
        }

        public void SendEmail2(string userEmail, string url)
        {
            var mailMessage = new MimeMessage();
            mailMessage.From.Add(MailboxAddress.Parse("sophie.kuhic89@ethereal.email"));
            mailMessage.To.Add(MailboxAddress.Parse(userEmail));

            mailMessage.Subject = "Confirm your email account";
           // mailMessage.IsBodyHtml = true;
            mailMessage.Body = new TextPart(TextFormat.Html) { Text = url} ;

            using var smtp = new MailKit.Net.Smtp.SmtpClient();
            smtp.Connect(_emailConfiguration.SmtpHost, _emailConfiguration.SmtpPort, SecureSocketOptions.StartTls);
            smtp.Authenticate(_emailConfiguration.SmtpUser, _emailConfiguration.SmtpPass);
            smtp.Send(mailMessage);
            smtp.Disconnect(true);

            //SmtpClient client = new SmtpClient();
            ////client.Credentials = new System.Net.NetworkCredential("sophie.kuhic89@ethereal.email", "2AaG8bbGjHzfKjvB7r");
            //client
            //client.Host = "smtp.ethereal.email";
            //client.Port = 587;
        }
    }
}
