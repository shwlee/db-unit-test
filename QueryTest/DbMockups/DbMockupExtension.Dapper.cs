using Dapper;
using InMemorySqliteTest.Contexts;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace QueryTest.DbMockups;

public static partial class DbMockupExtension
{
    public static void Seed(this IDbConnection connection)
    {
        connection.Open();

        CreateTables(connection);

        var tables = connection.Query<string>("SELECT name FROM sqlite_master");

        var blogs = CreateBlogs();
        var insertBlogs = @"INSERT INTO Blog VALUES(@BlogId, @Url, @Rating)";
        connection.Execute(insertBlogs, blogs.ToList());

        var insertedBlogs = connection.Query<Blog>("SELECT * FROM Blog");

        var posts = CreatePosts();
        var insertPosts = @"INSERT INTO Post VALUES(@PostId, @Title, @Content, @BlogId)";
        connection.Execute(insertPosts, posts.ToList());

        var insertedPosts = connection.Query<Blog>("SELECT * FROM Post");
    }

    private static void CreateTables(IDbConnection connection)
    {
        var createTablesCommand =
@"CREATE TABLE Blog
(
    [BlogId] [int] NOT NULL PRIMARY KEY,	
	[Url] [nvarchar](1024) NULL,
	[Rating] [bigint] NULL	
);

CREATE TABLE Post
(
    [PostId] [int] NOT NULL,	    
    [Title] [nvarchar](1024) NULL,
	[Content] [text] NULL,
    [BlogId] [int] NOT NULL,	
    CONSTRAINT fk_BlogId
        FOREIGN KEY (BlogId)
        REFERENCES Blog(BlogId)
);";
        connection.Execute(createTablesCommand);
    }

    private static IEnumerable<Blog> CreateBlogs()
    {
        yield return BlogTestBag.Blog1;
        yield return BlogTestBag.Blog2;
        yield return BlogTestBag.Blog3;
        yield return BlogTestBag.Blog4;
        yield return BlogTestBag.Blog5;
    }

    private static IEnumerable<Post> CreatePosts()
    {
        yield return PostTestBag.Post1;
        yield return PostTestBag.Post2;
        yield return PostTestBag.Post3;
        yield return PostTestBag.Post4;
        yield return PostTestBag.Post5;
        yield return PostTestBag.Post6;        
    }
}
