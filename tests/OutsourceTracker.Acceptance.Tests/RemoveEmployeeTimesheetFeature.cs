using Dapper;
using OutsourceTracker.Acceptance.Tests.Fixture;
using Spectre.Console.Testing;

namespace OutsourceTracker.Acceptance.Tests;

public class RemoveEmployeeTimesheetFeature(TestAppBuilder appBuilder) : IClassFixture<TestAppBuilder>
{
    private readonly CommandAppTester _app = appBuilder.Build();

    [Fact]
    public async Task Remove_existing_employee_timesheet()
    {
        var args = "remove Emilia".Split(' ');
        var initialTimesheetEntryCount = await CountTimesheetEntriesAsync();

        var result = await _app.RunAsync(args);

        result.Output.Should().Be("Timesheet deleted successfully");
        var timesheetEntryCountAfterRemove = await CountTimesheetEntriesAsync();
        var removedTimesheetEntryCount = initialTimesheetEntryCount - timesheetEntryCountAfterRemove;
        removedTimesheetEntryCount.Should().Be(20);
        (await TaskExists(28)).Should().Be(false);
        var timesheetHistoryEntries = await CountTimesheetHistoryEntriesAsync();
        timesheetHistoryEntries.Should().Be(20);
    }

    [Fact]
    public async Task Remove_non_existing_employee_timesheet_returns_error()
    {
        var args = "remove Test".Split(' ');

        var result = await _app.RunAsync(args);

        result.Output.Should().Be("Error: Employee not found");
    }

    [Fact]
    public async Task Remove_employee_with_no_timesheet_returns_error()
    {
        var args = "remove New".Split(' ');

        var result = await _app.RunAsync(args);

        result.Output.Should().Be("Error: Timesheet is empty");
    }

    private async Task<int> CountTimesheetEntriesAsync()
    {
        using var connection = await appBuilder.CreateConnectionAsync();
        return await connection.ExecuteScalarAsync<int>("SELECT COUNT(1) FROM TimeSpent;");
    }

    private async Task<int> CountTimesheetHistoryEntriesAsync()
    {
        using var connection = await appBuilder.CreateConnectionAsync();
        return await connection.ExecuteScalarAsync<int>($"SELECT COUNT(1) FROM TimesheetHistory;");
    }

    private async Task<bool> TaskExists(int taskId)
    {
        using var connection = await appBuilder.CreateConnectionAsync();
        var taskCount =
            await connection.ExecuteScalarAsync<int>($"SELECT COUNT(1) FROM Tasks t WHERE t.TaskId='{taskId}';");
        return taskCount == 1;
    }
}