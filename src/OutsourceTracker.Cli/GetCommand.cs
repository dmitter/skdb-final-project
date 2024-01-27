using System.Globalization;
using OutsourceTracker.Core;
using OutsourceTracker.Domain;
using Spectre.Console;
using Spectre.Console.Cli;

namespace OutsourceTracker.Cli;

public class GetCommand(string connectionString, IAnsiConsole console) : Command<GetSettings>
{
    public override int Execute(CommandContext context, GetSettings settings)
    {
        PrintEmployeeTimesheet(settings.EmployeeName);
        return 0;
    }

    private void PrintEmployeeTimesheet(string employeeName)
    {
        var timesheetService = new TimesheetService(connectionString);
        var result = timesheetService.GetEmployeeTimesheet(employeeName);
        if (!result.EmployeeExits)
        {
            console.WriteLine(@$"Employee ""{employeeName}"" not found");
            return;
        }

        PrintTimesheet(employeeName, result.Timesheet);
    }

    private void PrintTimesheet(string employeeName, IEnumerable<TimeSpent> timesheet)
    {
        var timesheetList = timesheet.ToList();
        if (timesheetList.Count == 0)
        {
            console.WriteLine(@$"No timesheet found for employee ""{employeeName}""");
            return;
        }

        PrintTable(timesheetList);
    }

    private void PrintTable(IEnumerable<TimeSpent> timesheet)
    {
        var table = new Table();

        table.AddColumn("#");
        table.AddColumn("Task");
        table.AddColumn("Start Time");
        table.AddColumn("End Time");

        var lineNumber = 1;
        foreach (var timeSpent in timesheet)
        {
            table.AddRow(lineNumber.ToString(),
                timeSpent.Task.Name,
                timeSpent.StartTime.ToString(CultureInfo.CurrentCulture),
                timeSpent.EndTime.ToString(CultureInfo.CurrentCulture));
            lineNumber++;
        }

        console.Write(table);
    }
}