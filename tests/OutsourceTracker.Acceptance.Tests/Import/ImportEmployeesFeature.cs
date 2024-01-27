using Dapper;
using OutsourceTracker.Acceptance.Tests.Fixture;
using Spectre.Console.Testing;

namespace OutsourceTracker.Acceptance.Tests.Import;

[Collection(nameof(ImportFeature))]
public class ImportEmployeesFeature(EmptyTestAppBuilder appBuilder) : IAsyncLifetime
{
    private readonly CommandAppTester _app = appBuilder.Build();

    public async Task InitializeAsync()
    {
        await ImportPositions();
    }

    [Fact]
    public async Task Imports_employees()
    {
        var args = "import employees.csv".Split(' ');
        var result = await _app.RunAsync(args);

        result.Output.Should().Be("Imported 99 employees");
    }

    [Fact]
    public async Task Does_not_import_duplicate_employees()
    {
        var args = "import employees.csv".Split(' ');
        await _app.RunAsync(args);

        var result = await _app.RunAsync(args);

        result.Output.Should().Contain("Imported 0 employees");
        result.Output.Should().Contain("Incorrect: 99");
    }

    public async Task DisposeAsync()
    {
        await CleanUpEmployeesTableAsync();
        await CleanUpPositionsTable();
    }

    private async Task ImportPositions()
    {
        var args = "import positions.csv".Split(' ');
        await _app.RunAsync(args);
    }

    private async Task CleanUpEmployeesTableAsync()
    {
        using var connection = await appBuilder.CreateConnectionAsync();
        await connection.ExecuteAsync("DELETE FROM Employees;\nALTER TABLE Employees AUTO_INCREMENT = 1;");
    }

    private async Task CleanUpPositionsTable()
    {
        using var connection = await appBuilder.CreateConnectionAsync();
        await connection.ExecuteAsync("DELETE FROM Positions;\nALTER TABLE Positions AUTO_INCREMENT = 1;");
    }
}