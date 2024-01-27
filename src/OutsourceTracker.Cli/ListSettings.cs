using Spectre.Console.Cli;

namespace OutsourceTracker.Cli;

public class ListSettings : CommandSettings
{
    [CommandArgument(0, "<EntityType>")] public required EntityType EntityType { get; init; }
}