using Library_Management_System.Core.Utility;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace Library_Management_System.Core.Extensions
{
   public static class ConfigurationExtension
    {

        public static EmailConfig GetEmailConfig(this IConfiguration configuration)
        {
            return new EmailConfig()
            {
                DisplayName = configuration.GetValue<string>("EmailConfiguration:UserName"),
                Email = configuration.GetValue<string>("EmailConfiguration:From"),
                Password = configuration.GetValue<string>("EmailConfiguration:Password")
            };
        }
    }
}
