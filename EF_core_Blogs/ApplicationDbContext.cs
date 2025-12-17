using EF_core_Blogs.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Text;

namespace EF_core_Blogs
{
    public class ApplicationDbContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Data Source=(localdb)\ProjectModels;Initial Catalog=Blogs;Integrated Security=True");
        }
        // Another way to add the AudiedEntry entity to the domain model (make a table) is to override the OnModelCreating method
        protected override void OnModelCreating(ModelBuilder modelBuilder) { 
            modelBuilder.Entity<AudiedEntry>();

            // If you want to ignore an entity and not create a table for it, you can use the Ignore method
            // modelBuilder.Ignore<Post>(); (code)

            // Build a table in database without migrations (that is the table will not be created when you run the migration commands)
            // it will build in database one time and after that it will not be affected by any migration commands
            // modelBuilder.Entity<Blog>().ToTable("Blogs",b => b.ExcludeFromMigrations()); (code)

            // Specify the table name for the Post entity with fluent API
            // modelBuilder.Entity<Post>().ToTable("Posts"); (code)

            // Specify the table name and schema for the Post entity with fluent API ((schema :) => it is a named parameter)
            // Schema => it is a way to logically group database objects such as tables, views, and stored procedures
            //  modelBuilder.Entity<Post>().ToTable("Posts", schema: "bloging"); (code)

            // Set the default schema for all tables in the context
            // modelBuilder.HasDefaultSchema("bloging");(code)

            // Ignore a specific property in the Blog entity
            // modelBuilder.Entity<Blog>().Ignore(p => p.dateTime); (code)

            // Specify the column name for the url property in the Blog entity with fluent API
            // modelBuilder.Entity<Blog>().Property(b => b.url).HasColumnName("Blogurl"); (code)

            // specify the column type for the Rating & url property in the Blog entity with fluent API
            // modelBuilder.Entity<Blog>(eb =>
            // {
            //   eb.Property(b => b.url).HasColumnType("varchar(200)").IsRequired();
            //    eb.Property(b => b.Rating).HasColumnType("decimal(5,2)");
            //}); (code)

            // spicfy the max length of the data that assign in the column
            // modelBuilder.Entity<Blog>().Property(b => b.url).HasMaxLength(200); (code)

            // write a comment on a column 
            // modelBuilder.Entity<Blog>().Property(b => b.Rating).HasComment("Rating the posts"); (code)

            // set a column as a primary key with fluent api
            // modelBuilder.Entity<Book>().HasKey(b => b.Bookkey)
            // .HasName("Bookkey"); (code)

            // Make a composite primary key
            // modelBuilder.Entity<Book>().HasKey(b => b.Bookkey);

            // Set a default value for a column with fluent API without using a sql built-in function (there is no data-anotation for it)
            // modelBuilder.Entity<Book>().Property(b => b.Rating).HasDefaultValue(2); (code)

            // set a default value by using a sql built in function (there is no data-anotation for it) 
            // modelBuilder.Entity<Book>().Property(b => b.PublishOn).HasDefaultValueSql("GETDATE()"); (code)

            // Computed Column : is a column that is not physically stored in the table, but is computed from other columns in the table
            // it is a virtual column that is calculated on the fly when you query the table depending on other columns
            // modelBuilder.Entity<Author>().Property(b => b.DisplayName).HasComputedColumnSql("[LastName] + ', ' +[FirstName]");

            // Specify that the Id property in the Category entity is an identity column with fluent API
            // This error appear : To change the IDENTITY property of a column, the column needs to be dropped and recreated.
            // you have to drop the column and recreate it with the identity property (delete the migration of that table and create a new one)
            modelBuilder.Entity<Category>().Property(c => c.Id).ValueGeneratedOnAdd();

        }
        public DbSet<Blog> Blogs { get; set; }

        public DbSet<Book> Books { get; set; }
        public DbSet<Author> Authors { get; set; }
        public DbSet<Category> Categories { get; set; }
        }
}
