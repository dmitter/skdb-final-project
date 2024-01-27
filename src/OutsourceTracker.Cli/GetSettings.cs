using Spectre.Console.Cli;

namespace OutsourceTracker.Cli;

public class GetSettings : CommandSettings
{
    [CommandArgument(0, "<EmployeeName>")] public required string EmployeeName { get; init; }
}