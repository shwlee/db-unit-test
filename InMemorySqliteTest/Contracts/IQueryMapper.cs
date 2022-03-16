using InMemorySqliteTest.Contexts;

namespace InMemorySqliteTest.Contracts;

public interface IQueryMapper
{
    Task<Blog?> GetBlogAsync(int blogId);

    Task<Post?> GetPostAsync(int postId);

    Task<IEnumerable<Post>?> GetPostsAsync(string title);

    Task<int> UpdatePostAsync(int postId, string updateTitle, string updateContent);
}
