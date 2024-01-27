using System.Data;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using OutsourceTracker.Cli;
using Spectre.Console.Testing;
using Testcontainers.MySql;

namespace OutsourceTracker.Acceptance.Tests.Fixture;

public class EmptyTestAppBuilder : IAsyncLifetime
{
    private readonly MySqlContainer _mysqlSqlContainer = new MySqlBuilder()
        .WithUsername("root")
        .Build();

    protected string ConnectionString => _mysqlSqlContainer.GetConnectionString();

    public CommandAppTester Build()
    {
        var appBuilder = new AppBuilder()
            .ConfigureAppConfiguration(config =>
                config.AddInMemoryCollection(new Dictionary<string, string?>
                {
                    [Config.ConnectionString] = ConnectionString
                }));
        return appBuilder.BuildTester();
    }

    public async Task InitializeAsync()
    {
        await _mysqlSqlContainer.StartAsync();
        await CreateDatabaseInitializer().InitializeAsync();
    }

    public Task DisposeAsync()
    {
        return _mysqlSqlContainer.DisposeAsync().AsTask();
    }

    public async Task<IDbConnection> CreateConnectionAsync()
    {
        var connection = new MySqlConnection(ConnectionString);
        await connection.OpenAsync();
        return connection;
    }

    protected virtual EmptyDatabaseInitializer CreateDatabaseInitializer()
    {
        return new EmptyDatabaseInitializer(ConnectionString);
    }
}