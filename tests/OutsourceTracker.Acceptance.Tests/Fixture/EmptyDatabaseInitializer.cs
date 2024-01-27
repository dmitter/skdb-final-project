using Dapper;
using MySql.Data.MySqlClient;

namespace OutsourceTracker.Acceptance.Tests.Fixture;

public class EmptyDatabaseInitializer(string connectionString)
{
    public virtual async Task InitializeAsync()
    {
        await using var connection = new MySqlConnection(connectionString);
        await connection.OpenAsync();
        var sql = await File.ReadAllTextAsync("schema.sql");
        await connection.ExecuteAsync(sql);
    }
}