using OutsourceTracker.Core;
using Spectre.Console;
using Spectre.Console.Cli;

namespace OutsourceTracker.Cli;

public class ListCommand(string connectionString, IAnsiConsole console) : Command<ListSettings>
{
    public override int Execute(CommandContext context, ListSettings settings)
    {
        if (settings.EntityType == EntityType.Employee)
        {
            ListEmployees();
            return 0;
        }

        return 1;
    }

    private void ListEmployees()
    {
        var table = new Table();

        table.AddColumn("Id");
        table.AddColumn("Name");
        table.AddColumn("Position");

        var employeeService = new EmployeeService(connectionString);
        foreach (var employee in employeeService.GetAllEmployees())
        {
            table.AddRow(employee.EmployeeId.ToString(), employee.Name, employee.Position.Name);
        }

        console.Write(table);
    }
}