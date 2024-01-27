using Dapper;
using OutsourceTracker.Acceptance.Tests.Fixture;
using Spectre.Console.Testing;

namespace OutsourceTracker.Acceptance.Tests.Import;

[Collection(nameof(ImportFeature))]
public class ImportTimesheetFeature(EmptyTestAppBuilder appBuilder) : IAsyncLifetime
{
    private readonly CommandAppTester _app = appBuilder.Build();

    public async Task InitializeAsync()
    {
        await ImportPositions();
        await ImportEmployees();
    }

    [Fact]
    public async Task Imports_timesheet()
    {
        File.Copy("timesheet_full.csv", "timesheet.csv", true);
        var args = "import timesheet.csv".Split(' ');
        var result = await _app.RunAsync(args);

        result.Output.Should().Contain("Imported 1725 timesheet entries");
        result.Output.Should().Contain("Incorrect: 40");
    }

    [Fact]
    public async Task Does_not_import_and_shows_error_when_start_gt_end()
    {
        const string data = "ANALYTICS-367,Anthony,2021-01-01 21:00:00,2021-01-01 18:00:00";
        await File.WriteAllTextAsync("timesheet.csv", data);
        var args = "import timesheet.csv".Split(' ');

        var result = await _app.RunAsync(args);

        result.Output.Should()
            .Contain(@"Start time ""01.01.2021 21:00:00"" is greater than end time ""01.01.2021 18:00:00""");
        result.Output.Should().Contain("Imported 0 timesheet entries");
        result.Output.Should().Contain("Incorrect: 1");
    }

    [Fact]
    public async Task Does_not_import_and_shows_error_on_overlapping_entries()
    {
        const string data = """
                            ANALYTICS-367,Anthony,2021-01-01 18:00:00,2021-01-01 21:00:00
                            ANALYTICS-367,Anthony,2021-01-01 19:00:00,2021-01-01 22:00:00
                            """;
        await File.WriteAllTextAsync("timesheet.csv", data);
        var args = "import timesheet.csv".Split(' ');

        var result = await _app.RunAsync(args);

        result.Output.Should()
            .Contain(
                @"Time range ""01.01.2021 19:00:00-01.01.2021 22:00:00"" intersects with time range ""01.01.2021 18:00:00-01.01.2021 21:00:00""");
        result.Output.Should().Contain("Imported 1 timesheet entries");
        result.Output.Should().Contain("Incorrect: 1");
    }

    public async Task DisposeAsync()
    {
        await CleanUpTimeSpentTable();
        await CleanUpEmployeesTableAsync();
        await CleanUpPositionsTable();
        await CleanUpTimesheetHistoryTable();
    }

    private async Task ImportPositions()
    {
        var args = "import positions.csv".Split(' ');
        await _app.RunAsync(args);
    }

    private async Task ImportEmployees()
    {
        var args = "import employees.csv".Split(' ');
        await _app.RunAsync(args);
    }

    private async Task CleanUpPositionsTable()
    {
        using var connection = await appBuilder.CreateConnectionAsync();
        await connection.ExecuteAsync("DELETE FROM Positions;\nALTER TABLE Positions AUTO_INCREMENT = 1;");
    }

    private async Task CleanUpEmployeesTableAsync()
    {
        using var connection = await appBuilder.CreateConnectionAsync();
        await connection.ExecuteAsync("DELETE FROM Employees;\nALTER TABLE Employees AUTO_INCREMENT = 1;");
    }

    private async Task CleanUpTimeSpentTable()
    {
        using var connection = await appBuilder.CreateConnectionAsync();
        await connection.ExecuteAsync("DELETE FROM TimeSpent;\nALTER TABLE TimeSpent AUTO_INCREMENT = 1;");
    }

    private async Task CleanUpTimesheetHistoryTable()
    {
        using var connection = await appBuilder.CreateConnectionAsync();
        await connection.ExecuteAsync(
            "DELETE FROM TimesheetHistory;\nALTER TABLE TimesheetHistory AUTO_INCREMENT = 1;");
    }
}