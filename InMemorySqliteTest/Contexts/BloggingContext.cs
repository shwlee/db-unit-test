using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InMemorySqliteTest.Contexts;

public class BloggingContext : DbContext
{
    public BloggingContext()
    {
    }

    public BloggingContext(DbContextOptions<BloggingContext> options)
        : base(options)
    {
    }

    public DbSet<Blog>? Blogs { get; set; }
    public DbSet<Post>? Posts { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Blog>()
         .HasMany(b => b.Posts)
         .WithOne();

        modelBuilder.Entity<Blog>()
            .Navigation(b => b.Posts)
            .UsePropertyAccessMode(PropertyAccessMode.Property);

        modelBuilder.Entity<Post>()
            .HasOne(p => p.Blog)
            .WithMany(b => b.Posts)
            .OnDelete(DeleteBehavior.Cascade);
    }

    public override void Dispose()
    {
        base.Dispose();
    }
}

[Table("Blog")]
public class Blog : IEquatable<Blog>
{
    [Key]
    public int BlogId { get; set; }
    public string Url { get; set; }
    public int Rating { get; set; }

    public List<Post>? Posts { get; set; }

    public bool Equals(Blog? other)
    {
        if (other is null)
        {
            return false;
        }

        return this.BlogId == other.BlogId &&
            string.CompareOrdinal(this.Url, other.Url) is 0 &&
            this.Rating == other.Rating;
    }
}

[Table("Post")]
public class Post : IEquatable<Post>
{
    [Key]
    public int PostId { get; set; }
    public string Title { get; set; }
    public string Content { get; set; }


    public int BlogId { get; set; }

    [ForeignKey("BlogId")]
    public Blog? Blog { get; set; }

    public bool Equals(Post? other)
    {
        if (other is null)
        {
            return false;
        }

        return this.PostId == other.PostId &&
            this.BlogId == other.BlogId &&
            string.CompareOrdinal(this.Title, other.Title) is 0 &&
            string.CompareOrdinal(this.Content, other.Content) is 0;
    }
}