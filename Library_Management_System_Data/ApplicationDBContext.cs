using Library_Management_System.Core.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Library_Management_System_Data
{
   public class ApplicationDBContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options) : base(options)
        {

        }
        public DbSet<Book> Books { get; set; }
        public DbSet<Author> Authors { get; set; }
        public DbSet<Borrowed_Book> Borrowed_Books { get; set; }
        public DbSet<Returned_Book> Returned_Books { get; set; }
        public DbSet<BookAuthor> BookAuthors { get; set; }
        public DbSet<UserBooks> UserBooks { get; set; }

        public DbSet<SelectedBook> SelectedBooks { get; set; }       // public DbSet<Role> Roles { get; set; }
    }
}
