using EF_core_Blogs.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
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
        // protected override void OnModelCreating(ModelBuilder modelBuilder) { 
        //    modelBuilder.Entity<AudiedEntry>();

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
        // modelBuilder.Entity<Book>().HasKey(b => b.Bookkey).HasName("Bookkey"); (code)

        // Make a composite primary key
        // modelBuilder.Entity<Book>().HasKey(b => b.Bookkey);(code)

        // Set a default value for a column with fluent API without using a sql built-in function (there is no data-anotation for it)
        // modelBuilder.Entity<Book>().Property(b => b.Rating).HasDefaultValue(2); (code)

        // set a default value by using a sql built in function (there is no data-anotation for it) 
        // modelBuilder.Entity<Book>().Property(b => b.PublishOn).HasDefaultValueSql("GETDATE()"); (code)

        // Computed Column : is a column that is not physically stored in the table, but is computed from other columns in the table
        // it is a virtual column that is calculated on the fly when you query the table depending on other columns
        // modelBuilder.Entity<Author>().Property(b => b.DisplayName).HasComputedColumnSql("[LastName] + ', ' +[FirstName]"); (code)

        // Specify that the Id property in the Category entity is an identity column with fluent API
        // This error appear : To change the IDENTITY property of a column, the column needs to be dropped and recreated.
        // you have to drop the column and recreate it with the identity property (delete the migration of that table and create a new one)
        // modelBuilder.Entity<Category>().Property(c => c.Id).ValueGeneratedOnAdd();

        // }

        // for relations between entities
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // One-to-One Relationship between Blog0 and BlogImage entities and specify the foreign key
            //modelBuilder.Entity<Blog0>()
            //    .HasOne(b => b.BlogImage)
            //    .WithOne(bi => bi.Blog)
            //    .HasForeignKey("BlogImage", "Blogforignkey");

            // One-to-Many Relationship between Blog0 and Post0 entities
            // there is no need to specify the foreign key here because EF Core will create it automatically for this relationship
            // there is no need to specify the navigation property for (Blog0) in the Post0 entity because EF Core will create it automatically
            // but it will be a single navigation property that is Posts0 in the Blog0 entity where i can access all the posts related to that blog
            // but can`t access the blog from the post directly

            // in this you can don`t mention the navigation property in the WithOne() method
            // if i make two navigation properties one in the principal entity and one in the dependent entity i have to mention them
            // both in the HasMany() and WithOne() methods.(using with the navigation property in the dependent entity and parent entity)
            //modelBuilder.Entity<Blog0>()
            //    .HasMany(b => b.Posts)
            //    .WithOne(p => p.Blog0);

            // can do it like this also (using with the navigation property in the dependent entity and parent entity)
            //modelBuilder.Entity<Post0>()
            //    .HasOne(p => p.Blog0)
            //    .WithMany(b => b.Posts);

            //using without the navigation property in the dependent entity and parent entity there is only foreign key
            // using the lamda expression when i mention the foreign key in the dependent entity 
            // or navigation property in the principal entity or navigation property in the dependent entity
            //modelBuilder.Entity<Post0>()
            //    .HasOne<Blog0>().
            //    WithMany().
            //    HasForeignKey(p => p.Blog0Id);

            // for one-to-many relationship between Car and RecordOfSales entities using foreign key that is not primary key
            // We must specify both the foreign key and the principal key in this case because EF Core cannot infer them automatically
            // because the foreign key is not referencing the default primary key of the principal entity
            // so the principal key must be specified explicitly to not allowing duplicate values in the LicensePlate column (act as a unique key)
            //modelBuilder.Entity<RecordOfSales>()
            //    .HasOne(r => r.Car)
            //    .WithMany(c => c.RecordsOfSales)
            //    .HasForeignKey(r => r.CarLicensePlate)
            //    .HasPrincipalKey(c => c.LicensePlate);

            // make a composite unique key on Car entity
            //modelBuilder.Entity<Car>()
            //    .HasMany(c => c.RecordsOfSales)
            //    .WithOne(r => r.Car)
            //    .HasForeignKey(s => new {s.CarLicensePlate, s.CarStatus })
            //    .HasPrincipalKey(c => new { c.LicensePlate, c.Status });
        }
        // public DbSet<Blog0> Blogs0 { get; set; }
        public DbSet<Car> Cars { get; set; }
    }
}
public class Blog0
{
    public int Id { get; set; }
    public string url { get; set; }
    
    // public BlogImage BlogImage { get; set; }
}
// for one-to-one relationship
public class BlogImage 
{
    public int Id { get; set; }
    // byte[] : is used to store binary data such as images, files, or any other type of data that can be represented as a sequence of bytes
    public byte[] Image { get; set; }
    public string Name { get; set; }
    public string caption { get; set; }
    
    // public int Blogforignkey { get; set; }
    // ForeignKey attribute is used to specify the foreign key property for a navigation property put on the navigation property of the 
    // parent entity
    // [ForeignKey("Blogforignkey")]
    // public Blog0 Blog { get; set; }
}

// for one-to-many relationship
public class Post0
{
    public int Id { get; set; }
    public string Title { get; set; }

    public int Blog0Id { get; set; }
}

// for one-to-many relationship
public class Car
{
    public int CarId { get; set; }
    public string Model { get; set; }
    public string LicensePlate { get; set; }
    public string Status { get; set; }
    public string Make { get; set; }
    public List<RecordOfSales> RecordsOfSales { get; set; }
}

public class RecordOfSales
{
    public int RecordOfSalesId { get; set; }
    public DateTime SaleDate { get; set; }
    public decimal Amount { get; set; }
    public string CarLicensePlate { get; set; }
    public string CarStatus { get; set; }
    public Car Car { get; set; }

}

