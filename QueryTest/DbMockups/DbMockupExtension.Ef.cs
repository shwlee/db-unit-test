using InMemorySqliteTest.Contexts;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace QueryTest.DbMockups;

public static partial class DbMockupExtension
{
    public static void Seed(this BloggingContext context)
    {
        var blogs = CreateBlogs();
        context.Blogs.AddRange(blogs);

        var posts = CreatePosts();
        context.Posts.AddRange(posts);

        context.SaveChanges();
    }
}
