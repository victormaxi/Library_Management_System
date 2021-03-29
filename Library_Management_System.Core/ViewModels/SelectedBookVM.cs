using Library_Management_System.Core.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Library_Management_System.Core.ViewModels
{
    public class SelectedBookVM
    {
        [Required]
        public string userId { get; set; }
       
        public virtual ICollection<BookVM> Books { get; set; }

        //[DisplayName("Id")]
        //public int SubjectId { get; set; }

        //[Required]
        //[DisplayName("Subject Name")]
        //public string Name { get; set; }

        //public virtual ICollection<BookViewModel> Books { get; set; }

    }
}
