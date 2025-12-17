using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace EF_core_Blogs.Models
{
    [NotMapped]
    public class Book
    {
        // Marking Bookkey as the primary key
        // [Key]
        public int Bookkey { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public int Rating { get; set; }
        public DateTime PublishOn { get; set; }
    }
}
