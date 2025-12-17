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
| Default Value | N/A | `HasDefaultValue(value)` |
| Default Value (SQL) | N/A | `HasDefaultValueSql("GETDATE()")` |
| Computed Column | N/A | `HasComputedColumnSql("[Col1] + [Col2]")` |
| Identity Column | `[DatabaseGenerated(DatabaseGeneratedOption.Identity)]` | `ValueGeneratedOnAdd()` |

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

### Default Values
```csharp
// Static default value
modelBuilder.Entity<Book>()
    .Property(b => b.Rating)
    .HasDefaultValue(2);

// SQL function default value
modelBuilder.Entity<Book>()
    .Property(b => b.PublishOn)
    .HasDefaultValueSql("GETDATE()");
```

### Computed Columns
```csharp
// Virtual column calculated from other columns
modelBuilder.Entity<Author>()
    .Property(a => a.DisplayName)
    .HasComputedColumnSql("[LastName] + ', ' + [FirstName]");
```
- Not physically stored in the table
- Calculated on-the-fly during queries
- Cannot be manually set in code
- Useful for derived data (full names, calculations, etc.)

### Identity Columns with Non-Int Types
**Important**: EF Core only recognizes `int` or `long` as primary keys by convention.

For other types like `byte`, `short`, etc., you must explicitly configure identity:

**Using Data Annotations:**
```csharp
[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
public byte Id { get; set; }
```

**Using Fluent API:**
```csharp
modelBuilder.Entity<Category>()
    .Property(c => c.Id)
    .ValueGeneratedOnAdd();
```

**Common Issue**: If you create a table without identity and try to add it later:
- Error: "To change the IDENTITY property of a column, the column needs to be dropped and recreated"
- Solution: Delete the migration and create a new one with the correct configuration

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
- **Book**: Demonstrates composite primary key, default values
- **Author**: Demonstrates computed columns (DisplayName)
- **Category**: Demonstrates identity column with byte type
- **AudiedEntry**: Registered via OnModelCreating method

---

## Best Practices Learned

- Use **Fluent API** for complex configurations (more powerful)
- Use **Data Annotations** for simple validations (more readable)
- Navigation properties enable automatic relationship discovery
- `[NotMapped]` excludes properties from database mapping
- Composite keys require Fluent API configuration
- **Always configure identity** for non-int primary key types
- Use computed columns for derived data instead of storing redundant values
- Set default values in database (not in code) for consistency

---

## Common Issues & Solutions

### 1. Computed Column Not Visible
- **Problem**: Column doesn't appear in View Data window
- **Solution**: Refresh database connection or query the data programmatically
- Computed columns are automatically populated by SQL Server

### 2. Primary Key Violation with Byte/Short Types
- **Problem**: Default value (0) inserted instead of auto-increment
- **Solution**: Add `[DatabaseGenerated(DatabaseGeneratedOption.Identity)]` or use `ValueGeneratedOnAdd()`

### 3. Migration Already Applied
- **Problem**: Changes in OnModelCreating don't reflect in database
- **Solution**: Create a new migration after configuration changes
- Use `Add-Migration <name>` then `Update-Database`

---

## Git Best Practices

### .gitignore for .NET Projects
Always exclude:
- `.vs/` - Visual Studio cache and user settings
- `bin/` and `obj/` - Build output folders
- `*.user`, `*.suo` - User-specific files
- `*.mdf`, `*.ldf` - SQL Server database files

### Branch Management
- Use `main` as the default branch (industry standard)
- Rename `master` to `main`: `git branch -m master main`

### Common Git Issues
- **Non-fast-forward error**: Remote has changes you don't have locally
  - Solution: `git pull origin main --rebase` then `git push`
- **Divergent branches**: Local and remote have different commits
  - Solution: Choose merge or rebase strategy: `git pull --no-rebase` or `git pull --rebase`

---

## Next Steps to Explore
- Foreign key configurations
- One-to-Many, Many-to-Many relationships
- Seeding data
- Query operations (LINQ)
- Change tracking
- Eager/Lazy loading
- Indexes and query optimization

---

**Database**: SQL Server LocalDB  
**Target Framework**: .NET 10  
**EF Core Version**: Latest
