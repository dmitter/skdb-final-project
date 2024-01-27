using OutsourceTracker.Core;
using Spectre.Console;
using Spectre.Console.Cli;

namespace OutsourceTracker.Cli;

public class ImportCommand(string connectionString, IAnsiConsole console) : Command<ImportSettings>
{
    public override int Execute(CommandContext context, ImportSettings settings)
    {
        if (string.Equals(settings.FileName, FileNames.Positions, StringComparison.OrdinalIgnoreCase))
        {
            Import(new PositionService(connectionString), settings.FileName, "positions");
            return 0;
        }

        if (string.Equals(settings.FileName, FileNames.Employees, StringComparison.OrdinalIgnoreCase))
        {
            Import(new EmployeeService(connectionString), settings.FileName, "employees");
            return 0;
        }

        if (string.Equals(settings.FileName, FileNames.Timesheet, StringComparison.OrdinalIgnoreCase))
        {
            Import(new TimesheetService(connectionString), settings.FileName, "timesheet entries");
            return 0;
        }

        return 1;
    }

    private void Import(IImportService importService, string filePath, string entitiesName)
    {
        AnsiConsole.Status().Start($"Importing {entitiesName}...", ctx =>
        {
            var importedCount = 0;
            var processingLineNumber = 1;
            var incorrectCount = 0;
            foreach (var result in importService.Import(filePath))
            {
                if (result.IsSuccessful)
                {
                    importedCount++;
                }
                else
                {
                    console.MarkupLine(
                        $"Error importing line [bold]{processingLineNumber}[/][bold]: {result.ErrorDescription}[/]");
                    incorrectCount++;
                }

                ctx.Status($"Imported [bold]{importedCount}[/] {entitiesName}...");
                processingLineNumber++;
            }

            console.MarkupLine($"Imported [bold]{importedCount}[/] {entitiesName}");
            if (incorrectCount > 0)
            {
                console.MarkupLine($"Incorrect: [bold]{incorrectCount}[/]");
            }
        });
    }
}