using System.Data;

namespace InMemorySqliteTest.Contracts;

public interface IDapperContextFactory
{
    IDbConnection CreateConnection(string connectionString);
}
