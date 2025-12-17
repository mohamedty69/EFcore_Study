using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace EF_core_Blogs.Models
{
    public class Category
    {
        // if the property not int the EF Core can`t recognize it as a primary key by convention
        // so you have to use the Data Annotations to specify the identity is a primary key
        // [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public byte Id { get; set; }
        public string Name { get; set; }
    }
}
