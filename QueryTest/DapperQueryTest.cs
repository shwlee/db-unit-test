using Dapper;
using FluentAssertions;
using InMemorySqliteTest.Contexts;
using InMemorySqliteTest.Contracts;
using InMemorySqliteTest.Services;
using Microsoft.Extensions.Configuration;
using Moq;
using QueryTest.DbMockups;
using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Xunit;

namespace QueryTest;
public class DapperQueryTest
{
    private const string InMemoryDataSource = "Data Source=:memory:";

    [Theory]
    [ClassData(typeof(BlogTestParametersById))]
    public async Task Test_Dapper_get_blog_by_id(int id, Blog comparer)
    {
        // Posts 는 Ef 에서 사용한다. 여기서는 비교 대상이 아니므로 제거한다.
        comparer.Posts = null;

        var queryMapper = GetDapperRepository();
        var blog = await queryMapper.GetBlogAsync(id);
        blog.Should().NotBeNull();
        blog.Should().BeEquivalentTo(comparer);
    }

    [Theory]
    [ClassData(typeof(PostTestParametersbyId))]
    public async Task Test_Dapper_get_post_by_id(int id, Post comparer)
    {
        var queryMapper = GetDapperRepository();
        var blog = await queryMapper.GetPostAsync(id);
        blog.Should().NotBeNull();
        blog.Should().BeEquivalentTo(comparer);
    }

    [Theory]
    [ClassData(typeof(PostTestParametersbyTitle))]
    public async Task Test_Dapper_get_posts_by_title_contains(string title, IEnumerable<Post> comparer)
    {
        var queryMapper = GetDapperRepository();
        var posts = await queryMapper.GetPostsAsync(title);
        posts.Should().NotBeNull();
        posts.Should().Equal(comparer);
    }

    [Theory]
    [ClassData(typeof(UpdatePostTestParameter))]
    public async Task Test_Dapper_update_posts_title_and_content_by_id(int postId, string updateTitle, string updateContent)
    {
        Post? comparer = null;
        var queryMapper = GetDapperRepository(connection =>
        {
            comparer = connection.QueryFirstOrDefault<Post>("SELECT * from Post WHERE PostId = @postId", new { postId });
        });

        var records = await queryMapper.UpdatePostAsync(postId, updateTitle, updateContent);

        // 여기서는 postId 기준으로 처리하므로 무조건 1개 행만 반환해야한다.
        records.Should().Be(1);

        comparer.Should().NotBeNull();
        comparer!.Title.Should().Be(updateTitle);
        comparer!.Content.Should().Be(updateContent);
    }

    private IQueryMapper GetDapperRepository([Optional] Action<IDbConnection?>? getComparer)
    {
        var contextFactory = new Mock<IDapperContextFactory>();
        contextFactory.Setup(f => f.CreateConnection(It.IsAny<string>())).Returns(() =>
        {
            var testConnection = new TestSqliteConnection(InMemoryDataSource, getComparer);
            testConnection.CreateFunction("getutcdate", () => DateTime.UtcNow);
            testConnection.Seed();

            return testConnection;
        });

        var mockConfSection = new Mock<IConfigurationSection>();
        mockConfSection.SetupGet(m => m[It.Is<string>(s => s == "DefaultConnectionString")])
            .Returns(InMemoryDataSource);

        var configuration = new Mock<IConfiguration>();
        configuration.Setup(config => config.GetSection(It.Is<string>(s => s == "ConnectionStrings")))
            .Returns(mockConfSection.Object);

        var repository = new DapperQuery(contextFactory.Object, configuration.Object);
        return repository;
    }
}