using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace EF_core_Blogs.Models
{
    public class Book
    {
        // Marking Bookkey as the primary key
        // [Key]
        public int Bookkey { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
    }
}
