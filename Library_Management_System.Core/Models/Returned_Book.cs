﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Library_Management_System.Core.Models
{
    public class Returned_Book
    {
        public int Id { get; set; }
        public Book Books { get; set; }
        public ApplicationUser ApplicationUsers { get; set; }
        public DateTime Returndate { get; set; }
    }
}
