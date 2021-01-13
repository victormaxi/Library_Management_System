using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Text;

namespace Library_Management_System.Core.Helper
{
    public class EmailHelper
    {
        public bool SendEmail(string userEmail, string confirmationLink)
        {
            MailMessage mailMessage = new MailMessage();
            mailMessage.From = new MailAddress("sophie.kuhic89@ethereal.email");
            mailMessage.To.Add(new MailAddress(userEmail));

            mailMessage.Subject = "Confirm your email account";
            mailMessage.IsBodyHtml = true;
            mailMessage.Body = confirmationLink;

            SmtpClient client = new SmtpClient();
            //client.Credentials = new System.Net.NetworkCredential("sophie.kuhic89@ethereal.email", "2AaG8bbGjHzfKjvB7r");
            //client
            //client.Host = "smtp.ethereal.email";
            //client.Port = 587;

            //// create message
            //var email = new MimeMessage();
            //email.From.Add(MailboxAddress.Parse(from ?? _appSettings.EmailFrom));
            //email.To.Add(MailboxAddress.Parse(to));
            //email.Subject = subject;
            //email.Body = new TextPart(TextFormat.Html) { Text = html };

            //// send email
            //using var smtp = new SmtpClient();
            //smtp.Connect(_appSettings.SmtpHost, _appSettings.SmtpPort, SecureSocketOptions.StartTls);
            //smtp.Authenticate(_appSettings.SmtpUser, _appSettings.SmtpPass);
            //smtp.Send(email);
            //smtp.Disconnect(true);

            try
            {
                client.Send(mailMessage);
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
