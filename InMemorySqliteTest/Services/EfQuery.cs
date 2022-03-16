using InMemorySqliteTest.Contexts;
using InMemorySqliteTest.Contracts;
using Microsoft.EntityFrameworkCore;

namespace InMemorySqliteTest.Services;
public class EfQuery : IQueryMapper
{
    private readonly IDbContextFactory<BloggingContext> _contextFactory;

    public EfQuery(IDbContextFactory<BloggingContext> contextFactory) => _contextFactory = contextFactory;

    public async Task<Blog?> GetBlogAsync(int blogId)
    {
        using var context = await _contextFactory.CreateDbContextAsync();
        var blog = await context.Blogs.FirstOrDefaultAsync(blog => blog.BlogId == blogId);
        return blog;
    }

    public async Task<Post?> GetPostAsync(int postId)
    {
        using var context = await _contextFactory.CreateDbContextAsync();
        var post = await context.Posts.FirstOrDefaultAsync(post => post.PostId == postId);
        return post;
    }

    public async Task<IEnumerable<Post>?> GetPostsAsync(string title)
    {
        using var context = await _contextFactory.CreateDbContextAsync();
        return await context.Posts.Where(p => p.Title.Contains(title)).ToListAsync();
    }

    public async Task<int> UpdatePostAsync(int postId, string updateTitle, string updateContent)
    {
        using var context = await _contextFactory.CreateDbContextAsync();
        var posts = context.Posts.Where(post => post.PostId == postId);
        foreach (var post in posts)
        {
            post.Title = updateTitle;
            post.Content = updateContent;
        }

        return await context.SaveChangesAsync();
    }
}
