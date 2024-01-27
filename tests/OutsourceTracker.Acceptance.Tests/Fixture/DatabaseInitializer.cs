using Dapper;
using MySql.Data.MySqlClient;

namespace OutsourceTracker.Acceptance.Tests.Fixture;

public class DatabaseInitializer(string connectionString) : EmptyDatabaseInitializer(connectionString)
{
    private readonly string _connectionString = connectionString;
    
    public override async Task InitializeAsync()
    {
        await base.InitializeAsync();
        await LoadDataAsync();
    }

    private async Task LoadDataAsync()
    {
        await using var connection = new MySqlConnection(_connectionString);
        await connection.OpenAsync();
        var sql = await File.ReadAllTextAsync("data.sql");
        await connection.ExecuteAsync(sql);
    }
}