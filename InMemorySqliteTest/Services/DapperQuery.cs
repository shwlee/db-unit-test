using Dapper;
using InMemorySqliteTest.Contexts;
using InMemorySqliteTest.Contracts;
using Microsoft.Extensions.Configuration;

namespace InMemorySqliteTest.Services;

public class DapperQuery : IQueryMapper
{
    private readonly IConfiguration _configuration;

    private readonly IDapperContextFactory _contextFactory;

    private readonly string _connectionString;

    public DapperQuery(IDapperContextFactory contextFactory, IConfiguration configuration)
    {
        _contextFactory = contextFactory;
        _configuration = configuration;
        _connectionString = _configuration.GetConnectionString("DefaultConnectionString");
    }

    public async Task<Blog?> GetBlogAsync(int blogId)
    {
        using var connection = _contextFactory.CreateConnection(_connectionString);
        connection.Open();

        var blog = await connection.QueryFirstOrDefaultAsync<Blog>("SELECT * FROM Blog WHERE BlogId = @blogId", new { blogId });
        return blog;
    }

    public async Task<Post?> GetPostAsync(int postId)
    {
        using var connection = _contextFactory.CreateConnection(_connectionString);
        connection.Open();

        var post = await connection.QueryFirstOrDefaultAsync<Post>("SELECT * FROM Post WHERE PostId = @postId", new { postId });
        return post;
    }

    public async Task<IEnumerable<Post>?> GetPostsAsync(string title)
    {
        using var connection = _contextFactory.CreateConnection(_connectionString);
        connection.Open();

        // like 검색은 파라미터에 포함시켜야 정상 동작한다.
        var post = await connection.QueryAsync<Post>("SELECT * FROM Post WHERE Title LIKE @title", new { title = $"%{title}%" });
        return post;
    }

    public async Task<int> UpdatePostAsync(int postId, string updateTitle, string updateContent)
    {
        using var connection = _contextFactory.CreateConnection(_connectionString);
        connection.Open();

        var command = @"UPDATE Post SET Title = @updateTitle, Content = @updateContent WHERE PostId = @postId";

        var post = await connection.ExecuteAsync(command, new { postId, updateTitle, updateContent });
        return post;
    }
}
