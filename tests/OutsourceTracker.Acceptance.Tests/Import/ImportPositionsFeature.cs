using Dapper;
using OutsourceTracker.Acceptance.Tests.Fixture;
using Spectre.Console.Testing;

namespace OutsourceTracker.Acceptance.Tests.Import;

[Collection(nameof(ImportFeature))]
public class ImportPositionsFeature(EmptyTestAppBuilder appBuilder) : IAsyncLifetime
{
    private readonly CommandAppTester _app = appBuilder.Build();

    public Task InitializeAsync()
    {
        return Task.CompletedTask;
    }

    [Fact]
    public async Task Imports_positions()
    {
        var args = "import positions.csv".Split(' ');

        var result = await _app.RunAsync(args);

        result.Output.Should().Be("Imported 7 positions");
    }

    [Fact]
    public async Task Does_not_import_duplicate_positions()
    {
        var args = "import positions.csv".Split(' ');
        await _app.RunAsync(args);

        var result = await _app.RunAsync(args);

        result.Output.Should().Contain("Imported 0 positions");
        result.Output.Should().Contain("Incorrect: 7");
    }

    public async Task DisposeAsync()
    {
        await CleanUpPositionsTable();
    }

    private async Task CleanUpPositionsTable()
    {
        using var connection = await appBuilder.CreateConnectionAsync();
        await connection.ExecuteAsync("DELETE FROM Positions;\nALTER TABLE Positions AUTO_INCREMENT = 1;");
    }
}