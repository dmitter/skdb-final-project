using Microsoft.EntityFrameworkCore;
using OutsourceTracker.Data;
using OutsourceTracker.Data.Csv;
using OutsourceTracker.Domain;

namespace OutsourceTracker.Core;

public class EmployeeService(string connectionString) : IImportService
{
    public IEnumerable<OperationResult> Import(string filePath)
    {
        using var csvRepository = new EmployeeCsvRepository(filePath);
        foreach (var employeeDto in csvRepository.GetItems()) yield return ImportEmployee(employeeDto);
    }

    public IEnumerable<Employee> GetAllEmployees()
    {
        using var dbContext = new OutsourceTrackerContext(connectionString);
        return dbContext.Employees
            .Include(x => x.Position)
            .OrderBy(x => x.Name)
            .ToList();
    }

    private OperationResult ImportEmployee(EmployeeDto employeeDto)
    {
        using var dbContext = new OutsourceTrackerContext(connectionString);

        var position = dbContext.Positions.FirstOrDefault(x => x.Name == employeeDto.Position);
        if (position == null) return OperationResult.Error($"""Position "{employeeDto.Position}" not found""");

        var employee = CreateEmployee(employeeDto.Name, position);
        dbContext.Employees.Add(employee);
        try
        {
            dbContext.SaveChanges();
            return OperationResult.Success();
        }
        catch (Exception ex)
        {
            return OperationResult.Error(ex);
        }
    }

    private static Employee CreateEmployee(string name, Position position)
    {
        return new Employee
        {
            Name = name,
            Position = position
        };
    }
}