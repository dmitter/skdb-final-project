using OutsourceTracker.Acceptance.Tests.Fixture;
using Spectre.Console.Testing;

namespace OutsourceTracker.Acceptance.Tests;

public class ListEmployeeFeature(TestAppBuilder appBuilder) : IClassFixture<TestAppBuilder>
{
    private readonly CommandAppTester _app = appBuilder.Build();

    [Fact]
    public async Task Lists_employees()
    {
        var args = "list employee".Split(' ');

        var result = await _app.RunAsync(args);

        await Verify(result.Output);
    }
}