using InMemorySqliteTest.Contexts;
using System.Collections;
using System.Collections.Generic;

namespace QueryTest.DbMockups;

internal static class BlogEfTestBag
{
    public static Blog Blog1 => new Blog
    {
        BlogId = 1,
        Rating = 100,
        Url = "https://blog.naver.com/vactorman1",
    };
    public static Blog Blog2 => new Blog
    {
        BlogId = 2,
        Rating = 200,
        Url = "https://blog.naver.com/vactorman2",
    };
    public static Blog Blog3 => new Blog
    {
        BlogId = 3,
        Rating = 300,
        Url = "https://blog.naver.com/vactorman3",
    };
    public static Blog Blog4 => new Blog
    {
        BlogId = 4,
        Rating = 400,
        Url = "https://blog.naver.com/vactorman4",
    };
    public static Blog Blog5 => new Blog
    {
        BlogId = 5,
        Rating = 500,
        Url = "https://blog.naver.com/vactorman5"
    };
}

internal static class PostEfTestBag
{
    public static Post Post1 => new Post { PostId = 1, BlogId = 1, Title = "DB unit sqlite test1", Content = "no contents1.", };
    public static Post Post2 => new Post { PostId = 2, BlogId = 1, Title = "DB unit content test2", Content = "no contents2.", };
    public static Post Post3 => new Post { PostId = 3, BlogId = 2, Title = "DB unit content test3", Content = "no contents3.", };
    public static Post Post4 => new Post { PostId = 4, BlogId = 2, Title = "DB unit test4", Content = "no contents4.",  };
    public static Post Post5 => new Post { PostId = 5, BlogId = 3, Title = "DB unit test511", Content = "no contents5.", };
    public static Post Post6 => new Post { PostId = 6, BlogId = 4, Title = "DB unit sqlitetest6", Content = "no contents6.",};
}


internal class BlogEfTestParametersById : IEnumerable<object[]>
{
    public IEnumerator<object[]> GetEnumerator()
    {
        yield return new object[] { 1, BlogTestBag.Blog1 };
        yield return new object[] { 2, BlogTestBag.Blog2 };
        yield return new object[] { 3, BlogTestBag.Blog3 };
        yield return new object[] { 4, BlogTestBag.Blog4 };
        yield return new object[] { 5, BlogTestBag.Blog5 };
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}

internal class PostEfTestParametersbyId : IEnumerable<object[]>
{
    public IEnumerator<object[]> GetEnumerator()
    {
        yield return new object[] { 1, PostTestBag.Post1 };
        yield return new object[] { 2, PostTestBag.Post2 };
        yield return new object[] { 3, PostTestBag.Post3 };
        yield return new object[] { 4, PostTestBag.Post4 };
        yield return new object[] { 5, PostTestBag.Post5 };
        yield return new object[] { 6, PostTestBag.Post6 };
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}

internal class PostEfTestParametersbyTitle : IEnumerable<object[]>
{
    public IEnumerator<object[]> GetEnumerator()
    {
        yield return new object[] { "1", new List<Post> { PostTestBag.Post1, PostTestBag.Post5 } };
        yield return new object[] { "content", new List<Post> { PostTestBag.Post2, PostTestBag.Post3 } };
        yield return new object[] { "sqlite", new List<Post> { PostTestBag.Post1, PostTestBag.Post6 } };
        yield return new object[] { "", new List<Post> { PostTestBag.Post1, PostTestBag.Post2, PostTestBag.Post3, PostTestBag.Post4, PostTestBag.Post5, PostTestBag.Post6 } };
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}

internal class UpdateEfPostTestParameter : IEnumerable<object[]>
{
    public IEnumerator<object[]> GetEnumerator()
    {
        yield return new object[] { 1, "update title", "it is content" };
        yield return new object[] { 2, "untitle", "there is content!!!" };
        yield return new object[] { 3, "3untitle", "test-content!!!" };
        yield return new object[] { 4, "untitle4", "44444444444!!!" };
        yield return new object[] { 5, "untitle5@!#$", "!@##@$%$%^&!!!" };
        yield return new object[] { 6, ";;;", ";;;!!!" };
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
