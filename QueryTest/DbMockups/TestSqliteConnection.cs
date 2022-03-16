using Microsoft.Data.Sqlite;
using System;
using System.Data;

namespace QueryTest.DbMockups;
public class TestSqliteConnection : SqliteConnection
{
    private Action<IDbConnection?>? _comaprer;

    public TestSqliteConnection(string? connectionString, Action<IDbConnection?>? getComparer) : base(connectionString)
        => _comaprer = getComparer;

    protected override void Dispose(bool disposing)
    {
        _comaprer?.Invoke(this);

        base.Dispose(disposing);
    }
}
