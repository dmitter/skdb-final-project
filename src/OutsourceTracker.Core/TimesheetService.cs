using Microsoft.EntityFrameworkCore;
using OutsourceTracker.Data;
using OutsourceTracker.Data.Csv;
using OutsourceTracker.Domain;
using Task = OutsourceTracker.Domain.Task;

namespace OutsourceTracker.Core;

public record GetEmployeeTimesheetResult(bool EmployeeExits, IEnumerable<TimeSpent> Timesheet);

public class TimesheetService(string connectionString) : IImportService
{
    public IEnumerable<OperationResult> Import(string filePath)
    {
        using var csvRepository = new TimesheetCsvRepository(filePath);
        foreach (var timesheetEntry in csvRepository.GetItems()) yield return ImportEntry(timesheetEntry);
    }

    public GetEmployeeTimesheetResult GetEmployeeTimesheet(string employeeName)
    {
        using var dbContext = new OutsourceTrackerContext(connectionString);
        using var transaction = dbContext.Database.BeginTransaction();

        var employee = dbContext.Employees.FirstOrDefault(x => x.Name == employeeName);
        if (employee is null)
        {
            return new GetEmployeeTimesheetResult(false, []);
        }

        var timesheet = dbContext.TimeSpent
            .Include(x => x.Task)
            .Include(x => x.Employee)
            .Where(x => x.Employee.Name == employeeName)
            .ToList();

        return new GetEmployeeTimesheetResult(true, timesheet);
    }

    public OperationResult RemoveEmployeeTimesheet(string employeeName)
    {
        using var dbContext = new OutsourceTrackerContext(connectionString);
        using var transaction = dbContext.Database.BeginTransaction();

        var employee = dbContext.Employees.FirstOrDefault(x => x.Name == employeeName);
        if (employee == null)
        {
            transaction.Rollback();
            return OperationResult.Error("Employee not found");
        }

        var timeSheets = dbContext.TimeSpent
            .Include(x => x.Task)
            .Where(x => x.EmployeeId == employee.EmployeeId)
            .ToList();

        if (timeSheets.Count == 0)
        {
            transaction.Rollback();
            return OperationResult.Error("Timesheet is empty");
        }

        var tasks = timeSheets.Select(x => x.Task).ToList();
        dbContext.TimeSpent.RemoveRange(timeSheets);
        dbContext.SaveChanges();

        foreach (var task in tasks)
        {
            var taskTimeSheetCount = dbContext.TimeSpent.Count(x => x.TaskId == task.TaskId);
            if (taskTimeSheetCount != 0) continue;

            dbContext.Tasks.Remove(task);
            dbContext.SaveChanges();
        }

        transaction.Commit();
        return OperationResult.Success();
    }

    private OperationResult ImportEntry(TimesheetEntryDto timesheetEntry)
    {
        using var dbContext = new OutsourceTrackerContext(connectionString);
        using var transaction = dbContext.Database.BeginTransaction();

        var employee = dbContext.Employees
            .Include(x => x.TimeSheet)
            .FirstOrDefault(x => x.Name == timesheetEntry.Employee);
        if (employee == null) return OperationResult.Error($"""Employee "{timesheetEntry.Employee}" not found""");

        var task = GetTask(timesheetEntry.Task, dbContext);

        var timeSpent = CreateTimeSpent(task, employee, timesheetEntry.StartTime, timesheetEntry.EndTime);
        if (!timeSpent.ValidateTimeRange())
        {
            transaction.Rollback();
            return OperationResult.Error(
                $"""
                 Start time "{timesheetEntry.StartTime}" is greater than end time "{timesheetEntry.EndTime}"
                 """);
        }

        var intersectingTimeSpent = employee.TimeSheet.FirstOrDefault(x => x.IntersectWith(timeSpent));
        if (intersectingTimeSpent != null)
        {
            transaction.Rollback();
            return OperationResult.Error(
                $"""
                 Time range "{timesheetEntry.StartTime}-{timesheetEntry.EndTime}" intersects with time range "{intersectingTimeSpent.StartTime}-{intersectingTimeSpent.EndTime}"
                 """);
        }

        employee.TimeSheet.Add(timeSpent);

        try
        {
            dbContext.SaveChanges();
        }
        catch (Exception ex)
        {
            transaction.Rollback();
            return OperationResult.Error("Error saving timesheet entry to database: ", ex);
        }

        transaction.Commit();
        return OperationResult.Success();
    }

    private static Task GetTask(string taskName, OutsourceTrackerContext dbContext)
    {
        var task = dbContext.Tasks.FirstOrDefault(x => x.Name == taskName);
        if (task != null) return task;

        task = new Task
        {
            Name = taskName
        };
        dbContext.Tasks.Add(task);
        return task;
    }

    private static TimeSpent CreateTimeSpent(Task task, Employee employee, DateTime startTime, DateTime endTime)
    {
        return new TimeSpent
        {
            Task = task,
            Employee = employee,
            StartTime = startTime,
            EndTime = endTime
        };
    }
}