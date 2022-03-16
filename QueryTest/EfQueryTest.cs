using FluentAssertions;
using InMemorySqliteTest.Contexts;
using InMemorySqliteTest.Contracts;
using InMemorySqliteTest.Services;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Moq;
using QueryTest.DbMockups;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace QueryTest;
public partial class QueryTest
{
    private const string InMemoryDataSource = "Data Source=:memory:";

    [Theory]
    [ClassData(typeof(BlogTestParametersById))]
    public async Task Test_Ef_get_blog_by_id(int id, Blog comparer)
    {
        comparer.Posts = null;
        var queryMapper = GetEfRepository(out _);
        var blog = await queryMapper.GetBlogAsync(id);
        blog.Should().NotBeNull();

        Assert.True(blog!.Equals(comparer));
    }

    [Theory]
    [ClassData(typeof(PostTestParametersbyId))]
    public async Task Test_Ef_get_post_by_id(int id, Post comparer)
    {
        var queryMapper = GetEfRepository(out _);
        var post = await queryMapper.GetPostAsync(id);
        post.Should().NotBeNull();

        Assert.True(post!.Equals(comparer));
    }

    [Theory]
    [ClassData(typeof(PostTestParametersbyTitle))]
    public async Task Test_Ef_get_posts_by_title_contains(string title, IEnumerable<Post> comparer)
    {
        var queryMapper = GetEfRepository(out _);
        var posts = await queryMapper.GetPostsAsync(title);
        posts.Should().NotBeNull();
        posts.Should().Equal(comparer);
    }

    [Theory]
    [ClassData(typeof(UpdatePostTestParameter))]
    public async Task Test_Ef_update_posts_title_and_content_by_id(int postId, string updateTitle, string updateContent)
    {
        Post? comparer = null;
        var queryMapper = GetEfRepository(out var context);
        context.Should().NotBeNull();
        context.Posts.Should().NotBeNull();

        comparer = context!.Posts!.FirstOrDefault(post => post.PostId == postId);
        comparer.Should().NotBeNull();

        var records = await queryMapper.UpdatePostAsync(postId, updateTitle, updateContent);

        // 여기서는 postId 기준으로 처리하므로 무조건 1개 행만 반환해야한다.
        records.Should().Be(1);

        comparer!.Title.Should().Be(updateTitle);
        comparer!.Content.Should().Be(updateContent);
    }

    private IQueryMapper GetEfRepository(out BloggingContext context)
    {
        try
        {
            var conn = new SqliteConnection(InMemoryDataSource);
            conn.Open(); // open connection to use

            var options = new DbContextOptionsBuilder<BloggingContext>()
               .UseSqlite(conn)
               .Options;

            context = new BloggingContext(options);

            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            context.Seed();

            var factory = new Mock<IDbContextFactory<BloggingContext>>();
            factory.Setup(f => f.CreateDbContextAsync(default(CancellationToken))).ReturnsAsync(context);

            var repository = new EfQuery(factory.Object);
            return repository;
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException(ex.Message);
        }
    }
}