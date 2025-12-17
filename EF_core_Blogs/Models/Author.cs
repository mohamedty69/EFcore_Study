using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace EF_core_Blogs.Models
{
    [NotMapped]
    public class Author
    {
        public int Id { get; set; }
        [Required,MaxLength(50)]
        public string FirstName { get; set; }
        [Required, MaxLength(50)]
        public string LastName { get; set; }
        [MaxLength(150),NotMapped]
        public string DisplayName { get; set; }
    }
}
