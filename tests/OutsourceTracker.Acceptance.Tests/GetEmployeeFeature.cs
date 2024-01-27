using OutsourceTracker.Acceptance.Tests.Fixture;
using Spectre.Console.Testing;

namespace OutsourceTracker.Acceptance.Tests;

public class GetEmployeeFeature(TestAppBuilder appBuilder) : IClassFixture<TestAppBuilder>
{
    private readonly CommandAppTester _app = appBuilder.Build();

    [Fact]
    public async Task Get_existing_employee_returns_timesheet()
    {
        var args = "get Emilia".Split(' ');

        var result = await _app.RunAsync(args);

        await Verify(result.Output);
    }

    [Fact]
    public async Task Get_non_existing_employee_returns_error()
    {
        var args = "get Test".Split(' ');

        var result = await _app.RunAsync(args);

        result.Output.Should().Be(@"Employee ""Test"" not found");
    }

    [Fact]
    public async Task Get_employee_with_no_timesheet_returns_empty_time_sheet_message()
    {
        var args = "get New".Split(' ');

        var result = await _app.RunAsync(args);

        result.Output.Should().Be(@"No timesheet found for employee ""New""");
    }
}