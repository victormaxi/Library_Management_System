using Library_Management_System.Core.Models;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Library_Management_System.Data
{
    public static class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            var context = serviceProvider.GetRequiredService<ApplicationDBContext>();
            
            context.Database.EnsureCreated();

            if (context.Books.Any())
            {
                var books = new Book[]
                {
                    new Book{BookId = 1, BookTitle = "Introduction to Physic"},
                    new Book{BookId = 2, BookTitle = "Art and Culture"}
                };
                foreach(Book b in books)
                {
                    context.Books.Add(b);
                }
                context.SaveChanges();

                var authors = new Author[]
                {
                    new Author{AuthorId = 1, AuthorName = "Njoku Samuel"},
                    new Author{AuthorId = 2, AuthorName = "Paul Ovie"}
                };
                foreach(Author a in authors)
                {
                    context.Authors.Add(a);
                }
                context.SaveChanges();

                var bookAuthor = new BookAuthor[]
                {
                    new BookAuthor{BookId = 1, AuthorId = 1},
                    new BookAuthor{BookId = 2, AuthorId = 2}
                };
                foreach(BookAuthor ba in bookAuthor)
                {
                    context.BookAuthors.Add(ba);
                }
                context.SaveChanges();

                
            };
        }
    }
}
