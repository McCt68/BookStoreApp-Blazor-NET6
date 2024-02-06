﻿using System;
using System.Collections.Generic;

namespace BookStoreApp.Api.Data
{
    public partial class Author
    {
        public Author()
        {
            Books = new HashSet<Book>();
        }

        // This reflect the Author table in the DB
        public int Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Bio { get; set; }

        // One Auther can have many books. So a Property which is a Collection of Book
        public virtual ICollection<Book> Books { get; set; }
    }
}
