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
| Foreign Key | `[ForeignKey("PropertyName")]` | `HasForeignKey(p => p.FK)` |
| Ignore Property | `[NotMapped]` | `Ignore(p => p.dateTime)` |
| Comment | `[Comment("text")]` | `HasComment("text")` |
| Default Value | N/A | `HasDefaultValue(value)` |
| Default Value (SQL) | N/A | `HasDefaultValueSql("GETDATE()")` |
| Computed Column | N/A | `HasComputedColumnSql("[Col1] + [Col2]")` |
| Identity Column | `[DatabaseGenerated(DatabaseGeneratedOption.Identity)]` | `ValueGeneratedOnAdd()` |

---

## Relationships in EF Core

### One-to-Many Relationship

#### Method 1: Using Both Navigation Properties (Bidirectional)
```csharp
// Principal Entity (Blog)
public class Blog0
{
    public int Id { get; set; }
    public List<Post0> Posts { get; set; }  // Collection navigation
}

// Dependent Entity (Post)
public class Post0
{
    public int Id { get; set; }
    public int Blog0Id { get; set; }  // Foreign key
    public Blog0 Blog0 { get; set; }  // Reference navigation
}

// Configuration
modelBuilder.Entity<Blog0>()
    .HasMany(b => b.Posts)
    .WithOne(p => p.Blog0);

// OR from the other side
modelBuilder.Entity<Post0>()
    .HasOne(p => p.Blog0)
    .WithMany(b => b.Posts);
```
- Both entities have navigation properties
- Relationship is defined on both sides

#### Method 2: Using Only Collection Navigation (Unidirectional)
```csharp
// Principal Entity
public class Blog0
{
    public int Id { get; set; }
    public List<Post0> Posts { get; set; }
}

// Dependent Entity (no navigation property to Blog)
public class Post0
{
    public int Id { get; set; }
    public int Blog0Id { get; set; }  // Only foreign key
}

// Configuration
modelBuilder.Entity<Blog0>()
    .HasMany(b => b.Posts)
    .WithOne();  // No navigation property specified
```
- You can access posts from blog, but NOT blog from post
- EF Core still creates the foreign key automatically

#### Method 3: Using Only Foreign Key (No Navigation Properties)
```csharp
// No navigation properties in either entity
public class Post0
{
    public int Id { get; set; }
    public int Blog0Id { get; set; }  // Foreign key only
}

// Configuration - must specify foreign key explicitly
modelBuilder.Entity<Post0>()
    .HasOne<Blog0>()
    .WithMany()
    .HasForeignKey(p => p.Blog0Id);
```
- No navigation properties in the entities
- Relationship is configured entirely through Fluent API

### One-to-One Relationship

```csharp
// Principal Entity
public class Blog0
{
    public int Id { get; set; }
    public BlogImage BlogImage { get; set; }
}

// Dependent Entity
public class BlogImage
{
    public int Id { get; set; }
    public int BlogForeignKey { get; set; }
    public Blog0 Blog { get; set; }
}

// Configuration with Fluent API
modelBuilder.Entity<Blog0>()
    .HasOne(b => b.BlogImage)
    .WithOne(bi => bi.Blog)
    .HasForeignKey<BlogImage>(bi => bi.BlogForeignKey);

// OR using Data Annotations
[ForeignKey("BlogForeignKey")]
public Blog0 Blog { get; set; }
```
- Each entity has a reference to the other
- Configuration sets up the one-to-one relationship

### Relationships with Non-Primary Key (Alternative Key)

**Important**: When foreign key doesn't reference the primary key of the principal entity.

```csharp
// Principal Entity
public class Car
{
    public int CarId { get; set; }  // Primary key
    public string LicensePlate { get; set; }  // Alternative unique key
    public List<RecordOfSales> RecordsOfSales { get; set; }
}

// Dependent Entity
public class RecordOfSales
{
    public int RecordOfSalesId { get; set; }
    public string CarLicensePlate { get; set; }  // FK to LicensePlate (not CarId!)
    public Car Car { get; set; }
}

// Configuration - Must specify BOTH foreign key AND principal key
modelBuilder.Entity<RecordOfSales>()
    .HasOne(r => r.Car)
    .WithMany(c => c.RecordsOfSales)
    .HasForeignKey(r => r.CarLicensePlate)
    .HasPrincipalKey(c => c.LicensePlate);
```
- Principal entity has an alternative key
- Must explicitly configure both foreign key and principal key in Fluent API

### Composite Foreign Keys

```csharp
public class Car
{
    public int CarId { get; set; }
    public string LicensePlate { get; set; }
    public string Status { get; set; }
    public List<RecordOfSales> RecordsOfSales { get; set; }
}

public class RecordOfSales
{
    public int RecordOfSalesId { get; set; }
    public string CarLicensePlate { get; set; }
    public string CarStatus { get; set; }
    public Car Car { get; set; }
}

// Configuration - Composite unique key and composite foreign key
modelBuilder.Entity<Car>()
    .HasMany(c => c.RecordsOfSales)
    .WithOne(r => r.Car)
    .HasForeignKey(s => new { s.CarLicensePlate, s.CarStatus })
    .HasPrincipalKey(c => new { c.LicensePlate, c.Status });
```
- Foreign key consists of multiple columns
- Must configure composite keys and foreign keys in Fluent API

---

## Important Relationship Tips

### Navigation Properties
- **Collection Navigation**: `List<Post>` or `ICollection<Post>` (one-to-many)
- **Reference Navigation**: `Blog Blog` (many-to-one or one-to-one)
- EF Core auto-discovers relationships from navigation properties
- You can have navigation on one side, both sides, or neither side

### Foreign Key Naming Conventions
EF Core automatically recognizes foreign keys if named:
- `<NavigationPropertyName>Id` (e.g., `BlogId` for `Blog` navigation)
- `<NavigationPropertyName><PrimaryKeyName>` (e.g., `BlogBlogId`)
- `<PrincipalEntityName>Id` (e.g., `BlogId` for Blog entity)

### Common Mistake: Duplicate Foreign Keys
**Problem**: EF Core creates two foreign keys for the same relationship

**Cause**: Not specifying navigation property in Fluent API when it exists in entity
```csharp
// WRONG - Creates two FKs
.WithOne()  // EF Core creates one FK
public Blog0 Blog0 { get; set; }  // EF Core creates another FK

// CORRECT - Specify the navigation property
.WithOne(p => p.Blog0)  // EF Core knows they're the same relationship
```
- Always specify the navigation property in Fluent API if it exists in your entity

### Cascade Delete Behavior
```csharp
.OnDelete(DeleteBehavior.Cascade)     // Delete dependent when principal deleted
.OnDelete(DeleteBehavior.Restrict)    // Prevent deletion if dependents exist
.OnDelete(DeleteBehavior.SetNull)     // Set FK to null when principal deleted
.OnDelete(DeleteBehavior.NoAction)    // No automatic action
```
Default: `Cascade` for required relationships, `SetNull` for optional

---

## Important Tips

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
- **Blog0/Post0**: Demonstrates one-to-many relationship configurations
- **BlogImage**: Demonstrates one-to-one relationship
- **Car/RecordOfSales**: Demonstrates foreign key to alternative (non-primary) key
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
- **Always specify navigation properties in Fluent API** if they exist in your entities
- Use `HasPrincipalKey` when foreign key references non-primary key

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

### 4. Duplicate Foreign Keys Created
- **Problem**: EF Core creates two foreign keys for the same relationship (e.g., `Blog0Id` and `Blog0Id1`)
- **Cause**: Navigation property exists in entity but not specified in Fluent API
- **Solution**: Specify the navigation property: `.WithOne(p => p.Blog0)` instead of `.WithOne()`

### 5. Alternative Key Relationship Errors
- **Problem**: Cannot create relationship with non-primary key
- **Solution**: Must use both `HasForeignKey()` AND `HasPrincipalKey()`
- EF Core needs explicit guidance when not using primary key

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
- Many-to-Many relationships
- Seeding data
- Query operations (LINQ)
- Change tracking
- Eager/Lazy loading
- Indexes and query optimization
- Shadow properties
- Owned entities

---

**Database**: SQL Server LocalDB  
**Target Framework**: .NET 10  
**EF Core Version**: Latest
