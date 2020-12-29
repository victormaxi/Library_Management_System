using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Library_Management_System.Core.ViewModels
{
    public class ApplicationUserVM
    {
        //public int Id { get; set; }
        [Required]
        [StringLength(50)]
        public string UserName { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [Phone]
        public string PhoneNumber { get; set; }
        [Required]
        [StringLength(50,MinimumLength = 5)]
        public string Password { get; set; } 
        [StringLength(50,MinimumLength = 5)]
        public string ConfirmPassword { get; set; }
    }
}
