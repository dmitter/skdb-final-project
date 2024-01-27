using OutsourceTracker.Acceptance.Tests.Fixture;
using Spectre.Console.Testing;

namespace OutsourceTracker.Acceptance.Tests;

public class ReportFeature(TestAppBuilder appBuilder) : IClassFixture<TestAppBuilder>
{
    private readonly CommandAppTester _app = appBuilder.Build();

    [Fact]
    public async Task top5longTasks_report()
    {
        var args = "report top5longTasks".Split(' ');

        var result = await _app.RunAsync(args);

        result.ExitCode.Should().Be(0);
        await Verify(result.Output);
    }

    [Fact]
    public async Task top5costTasks_report()
    {
        var args = "report top5costTasks".Split(' ');

        var result = await _app.RunAsync(args);

        result.ExitCode.Should().Be(0);
        await Verify(result.Output);
    }

    [Fact]
    public async Task top5employees_report()
    {
        var args = "report top5employees".Split(' ');

        var result = await _app.RunAsync(args);

        result.ExitCode.Should().Be(0);
        await Verify(result.Output);
    }
}