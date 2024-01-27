using Spectre.Console.Cli;

namespace OutsourceTracker.Cli;

public class ReportSettings : CommandSettings
{
    [CommandArgument(0, "<ReportName>")] public required string ReportName { get; init; }
}