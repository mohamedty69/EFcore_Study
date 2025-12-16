using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using System.Xml.Serialization;

namespace EF_core_Blogs.Models
{
    // Data Annotations to specify the table name in the database
    // [Table("Posts")]
    public class Post
    {
        public int Id { get; set; }
        // write a comment on a specific column
        // [Comment("First comment")]
        public string Title { get; set; }
        public string Content { get; set; }
        // Navigation property (property to relate to Blog) => the navigation property is a property that holds a reference to another
        // entity (domain model) in a relationship. It allows you to navigate from one entity to another.
         
        public Blog Blog { get; set; }
    }
}
