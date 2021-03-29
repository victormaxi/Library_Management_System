using System.ComponentModel.DataAnnotations;

namespace Library_Management_System.Core.Models
{
    public class AuthenticateModel
    {
        [Required]
        public string UserName { get; set;}

        public string Password { get; set;}
    }
}