using OutsourceTracker.Core;
using Spectre.Console;
using Spectre.Console.Cli;

namespace OutsourceTracker.Cli;

public class RemoveCommand(string connectionString, IAnsiConsole console) : Command<RemoveSettings>
{
    public override int Execute(CommandContext context, RemoveSettings settings)
    {
        return RemoveEmployeeTimesheet(settings.EmployeeName);
    }

    private int RemoveEmployeeTimesheet(string employeeName)
    {
        var employeeService = new TimesheetService(connectionString);
        var result = employeeService.RemoveEmployeeTimesheet(employeeName);
        if (!result.IsSuccessful)
        {
            console.MarkupLine($"Error: [bold]{result.ErrorDescription}[/]");
            return 1;
        }

        console.WriteLine("Timesheet deleted successfully");
        return 0;
    }
}