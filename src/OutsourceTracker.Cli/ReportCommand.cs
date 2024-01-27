using OutsourceTracker.Core;
using Spectre.Console;
using Spectre.Console.Cli;

namespace OutsourceTracker.Cli;

public class ReportCommand(string connectionString, IAnsiConsole console) : Command<ReportSettings>
{
    public override int Execute(CommandContext context, ReportSettings settings)
    {
        if (string.Equals(settings.ReportName, "top5longTasks", StringComparison.OrdinalIgnoreCase))
        {
            PrintTop5LongTasks();
            return 0;
        }

        if (string.Equals(settings.ReportName, "top5costTasks", StringComparison.OrdinalIgnoreCase))
        {
            PrintTop5CostTasks();
            return 0;
        }

        if (string.Equals(settings.ReportName, "top5employees", StringComparison.OrdinalIgnoreCase))
        {
            PrintTop5Employees();
            return 0;
        }

        return 1;
    }

    private void PrintTop5LongTasks()
    {
        var table = new Table();

        table.AddColumn("#");
        table.AddColumn("Task");
        table.AddColumn("Hours Spent");

        var lineNumber = 1;
        var reportService = new ReportService(connectionString);
        foreach (var item in reportService.GetTop5LongTasks())
        {
            table.AddRow(lineNumber.ToString(), item.Task, item.TotalHoursSpent.ToString());
            lineNumber++;
        }

        console.Write(table);
    }

    private void PrintTop5CostTasks()
    {
        var table = new Table();

        table.AddColumn("#");
        table.AddColumn("Task");
        table.AddColumn("Total Costs");

        var lineNumber = 1;
        var reportService = new ReportService(connectionString);
        foreach (var item in reportService.GetTop5CostTasks())
        {
            table.AddRow(lineNumber.ToString(), item.Task, item.TotalCosts.ToString());
            lineNumber++;
        }

        console.Write(table);
    }

    private void PrintTop5Employees()
    {
        var table = new Table();

        table.AddColumn("#");
        table.AddColumn("Employee");
        table.AddColumn("Hours Spent");

        var lineNumber = 1;
        var reportService = new ReportService(connectionString);
        foreach (var item in reportService.GetTop5Employees())
        {
            table.AddRow(lineNumber.ToString(), item.Name, item.TotalHoursSpent.ToString());
            lineNumber++;
        }

        console.Write(table);
    }
}