using OutsourceTracker.Cli;
using Spectre.Console.Testing;

namespace OutsourceTracker.Acceptance.Tests.Fixture;

public static class AppBuilderExtensions
{
    public static CommandAppTester BuildTester(this AppBuilder appBuilder)
    {
        appBuilder.Build();
        var app = new CommandAppTester(appBuilder.Registrar);
        app.Configure(appBuilder.Configuration);
        return app;
    }
}