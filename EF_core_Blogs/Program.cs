using System;
using System.Collections.Generic;
using EF_core_Blogs.Models;

namespace EF_core_Blogs;

class Program
{
    static void Main(string[] args)
    {
        var _context = new ApplicationDbContext();
        // Add a new category to the database 
        // and if you try to add the same category again it will throw an exception because the primary key is byte and the value is 0
        // which is the default value for byte type and it don`t use the identity column feature of the database so it will try to insert the same value again
        // that will cause a primary key violation exception (duplicate key)
        // to solve it make the identity property for the primary key in the OnModelCreating method of the ApplicationDbContext class or with data annotations
        var cat = new Category { Name = "Science Fiction" };
        _context.Categories.Add(cat);
        _context.SaveChanges();

    }
}
