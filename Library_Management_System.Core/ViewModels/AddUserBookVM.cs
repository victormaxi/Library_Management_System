using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Library_Management_System.Core.ViewModels
{
    public class AddUserBookVM
    {
        [Required]
        public int BookId { get; set; }
        [Required]
        public string  ApplicationUserId { get; set; }
    }
}
