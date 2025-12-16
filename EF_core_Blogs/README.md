# Entity Framework Core Study Notes

## Project Overview
This project demonstrates core EF Core concepts including entity configuration, relationships, and migrations using SQL Server LocalDB.

---

## Key Concepts Covered

### 1. DbContext Configuration
```csharp
// Connection string in OnConfiguring
optionsBuilder.UseSqlServer(@"Data Source=(localdb)\ProjectModels;Initial Catalog=Blogs;...")
```

### 2. Entity Registration (3 Ways)
- **DbSet Property**: `public DbSet<Blog> Blogs { get; set; }`
- **OnModelCreating**: `modelBuilder.Entity<AudiedEntry>();`
- **Auto-discovery**: Navigation properties (e.g., `List<Post>`) are auto-registered

---

## Configuration Methods

### Data Annotations vs Fluent API

| Feature | Data Annotations | Fluent API |
|---------|-----------------|------------|
| Table Name | `[Table("Posts")]` | `ToTable("Posts")` |
| Column Name | `[Column("Blogurl")]` | `HasColumnName("Blogurl")` |
| Data Type | `[Column(TypeName = "varchar(200)")]` | `HasColumnType("varchar(200)")` |
| Max Length | `[MaxLength(200)]` | `HasMaxLength(200)` |
| Primary Key | `[Key]` | `HasKey(b => b.Bookkey)` |
| Ignore Property | `[NotMapped]` | `Ignore(p => p.dateTime)` |
| Comment | `[Comment("text")]` | `HasComment("text")` |

---

## Important Tips

### Navigation Properties
- Define relationships without manual FK configuration
- EF Core auto-discovers related entities
- Example: `public List<Post> Posts { get; set; }` in Blog model

### Composite Primary Keys
```csharp
modelBuilder.Entity<Book>().HasKey(b => new { b.Bookkey, b.Author });
```

### Schema Organization
```csharp
// Set schema for specific table
.ToTable("Posts", schema: "blogging")

// Set default schema for all tables
modelBuilder.HasDefaultSchema("blogging");
```

### Exclude from Migrations
```csharp
// Create table directly in DB without migrations
modelBuilder.Entity<Blog>().ToTable("Blogs", b => b.ExcludeFromMigrations());
```

### Ignore Entities
```csharp
modelBuilder.Ignore<Post>(); // Won't create table
```

---

## Models Created

- **Blog**: Main entity with `url`, `Rating`, and navigation to Posts
- **Post**: Related entity with `Title`, `Content`, and navigation to Blog
- **Book**: Demonstrates composite primary key
- **AudiedEntry**: Registered via OnModelCreating method

---

## Best Practices Learned

- Use **Fluent API** for complex configurations (more powerful)
- Use **Data Annotations** for simple validations (more readable)
- Navigation properties enable automatic relationship discovery
- `[NotMapped]` excludes properties from database mapping
- Composite keys require Fluent API configuration

---

## Next Steps to Explore
- Foreign key configurations
- One-to-Many, Many-to-Many relationships
- Seeding data
- Query operations (LINQ)
- Change tracking
- Eager/Lazy loading

---

**Database**: SQL Server LocalDB  
**Target Framework**: .NET 10  
**EF Core Version**: Latest
