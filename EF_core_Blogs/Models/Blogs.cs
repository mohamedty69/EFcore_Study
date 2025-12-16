using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Security.Cryptography;
using System.Text;

namespace EF_core_Blogs.Models
{
    public class Blog
    {
        public int Id { get; set; }

        public decimal Rating { get; set; }

        // Data Annotations to specify the column name in the database
        // [Column("Blogurl")]
        // Data Annotations to specify the data type of the column in the database
        // [Column(TypeName = "varchar(200)")]
        // Data Annotations to specify the maximum length of the column in the database
        // [MaxLength(200)]
        public string url { get; set; }

        // This property will not be mapped to the database
        [NotMapped]
        public DateTime dateTime { get; set; }
        // Navigation property (property to relate to Post) => the navigation property is a property that holds a reference to another domain model
        // in a relationship.
        // - I don`t need to make a reference for Posts class in ApplicationDbContext.cs because EF Core will automatically create the relationship
        // [NotMapped]
        public List<Post> Posts { get; set; }
    }
}
