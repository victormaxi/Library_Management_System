//using Library_Management_System.Core.Interfaces;
//using Library_Management_System.Core.Models;
//using Microsoft.Extensions.Configuration;
//using MimeKit;
//using SendGrid;
//using SendGrid.Helpers.Mail;
//using MailKit.Net.Smtp;
//using System;
//using System.Collections.Generic;
//using System.Net.Http;

//using System.Text;
//using System.Threading.Tasks;
//using System.Net.Mail;
//using Library_Management_System.Core.Utility;
//using SmtpClient = System.Net.Mail.SmtpClient;
//using System.Net;
//using Microsoft.Extensions.Logging;

//namespace Library_Management_System.Domain.Services
//{
//    public class MailServicesManager : IMailServices
//    {
//        private IConfiguration _configuration;
//        private readonly EmailConfiguration _emailConfiguration;
//        private readonly ILogger<MailServicesManager> _logger;

//        public MailServicesManager(IConfiguration configuration, EmailConfiguration emailConfiguration, ILogger<MailServicesManager> logger)
//        {
//            _logger = logger;
//            _configuration = configuration;
//            _emailConfiguration = emailConfiguration;
//        }
//        //public async Task SendEmailAsync(string toEmail, string subject, string content)
//        //{
//        //    try
//        //    {


//        //    }

//        //    catch (Exception ex)
//        //    {
//        //        throw new Exception(ex.Message);
//        //    }
//        //}

//        //private MimeMessage CreateEmailMessage(EmailMessage message, string url)
//        //{
//        //    var emailMessage = new MimeMessage();
//        //    emailMessage.From.Add(new MailboxAddress(_emailConfiguration.From));
//        //    emailMessage.To.AddRange(message.To);
//        //    emailMessage.Subject = message.Subject;
//        //    emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Text) { Text = url};
//        //   // emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Text) { Text = message.Content };

//        //    return emailMessage;
//        //}
//        private MimeMessage CreateEmailMessage(string userEmail, string url)
//        {
//            var emailMessage = new MimeMessage();
//            emailMessage.From.Add(new MailboxAddress(_emailConfiguration.From));
//            emailMessage.To.Add(new MailboxAddress(userEmail));
//            emailMessage.Subject = "Confirm Your Email Account";
//            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Text) { Text = url};
//           // emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Text) { Text = message.Content };

//            return emailMessage;
//        }
//        //public MimeMessage(IEnumerable<InternetAddress> from, IEnumerable<InternetAddress> to, string subject, MimeEntity body);
//        private void Send (MimeMessage message)
//        {
//            using  (var client = new MailKit.Net.Smtp.SmtpClient())
//            {
//                try
//                {
//                    client.Connect(_emailConfiguration.SmtpServer, _emailConfiguration.Port, true);
//                    client.AuthenticationMechanisms.Remove("XOAUTH2");
//                    client.Authenticate(_emailConfiguration.UserName, _emailConfiguration.Password);

//                    client.Send(message);
//                }
//                catch(Exception ex)
//                {
//                    throw new Exception(ex.Message);
//                }
//                finally
//                {
//                    client.Disconnect(true);
//                    client.Dispose();
//                }
//            }
//        }

//       // public void SendEmail(EmailMessage emailMessage, string url)
//       // {
//       //     var message = CreateEmailMessage(emailMessage, url);

//       //     Send(message);
//       //}

//        public void SendEmail(string emailMessage, string url)
//        {
//            var message = CreateEmailMessage(emailMessage, url);

//            Send(message);
//        }

//        public Task SendEmailAsync(string toEmail, string subject, string content)
//        {
//            throw new NotImplementedException();
//        }

//        public async Task<bool> SendMail(string email, string subject, string body, EmailConfig emailConfig)
//        {
//            try
//            {
//                var mailFrom = new MailAddress(emailConfig.Email, emailConfig.DisplayName);
//                var mailTo = new MailAddress(email);

//                MailMessage mailMsg = new MailMessage(mailFrom, mailTo);
//                mailMsg.Subject = subject;

//                mailMsg.Body = body;

//                mailMsg.IsBodyHtml = true;
//                SmtpClient smtp = new SmtpClient(emailConfig.Host, int.Parse(emailConfig.Port));
//                //smtp.Timeout = 2000;
//                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
//                smtp.UseDefaultCredentials = false;
//                smtp.Credentials = new NetworkCredential(emailConfig.Email, emailConfig.Password);
//                smtp.EnableSsl = true;
//                await smtp.SendMailAsync(mailMsg);

//                return true;
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex.Message);
//                return false;
//            }

//        }
//        //public void SendEmail(EmailMessage emailMessage)
//        //{
//        //    throw new NotImplementedException();
//        //}

//        //public void SendEmail2(EmailMessage emailMessage, string url)
//        //{
//        //    throw new NotImplementedException();
//        //}
//    }
//}
