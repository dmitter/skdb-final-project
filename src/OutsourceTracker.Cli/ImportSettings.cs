using Spectre.Console.Cli;

namespace OutsourceTracker.Cli;

public class ImportSettings : CommandSettings
{
    [CommandArgument(0, "<FileName>")] public required string FileName { get; init; }
}