using Microsoft.EntityFrameworkCore;
using OutsourceTracker.Data;

namespace OutsourceTracker.Core;

public class ReportService(string connectionString)
{
    public IEnumerable<TaskTimeCosts> GetTop5LongTasks()
    {
        using var dbContext = CreateDbContext();

        return dbContext.Database
            .SqlQuery<TaskTimeCosts>($"""
                                      with TimeStats as (select TaskId, TIMESTAMPDIFF(HOUR, StartTime, EndTime) as HoursSpent from TimeSpent ts)
                                      select Name as Task, SUM(HoursSpent) as TotalHoursSpent
                                      from TimeStats s
                                      join Tasks t on t.TaskId=s.TaskId
                                      group by s.TaskId
                                      order by 2 desc, 1 limit 5;
                                      """
            )
            .ToList();
    }

    public IEnumerable<TaskMoneyCosts> GetTop5CostTasks()
    {
        using var dbContext = CreateDbContext();

        return dbContext.Database
            .SqlQuery<TaskMoneyCosts>($"""
                                       with CostStats as (select TaskId, TIMESTAMPDIFF(HOUR, StartTime, EndTime) * p.HourlyRate as Cost from TimeSpent ts
                                       join Employees e on ts.EmployeeId=e.EmployeeId
                                       join Positions p on p.PositionId=e.PositionId)
                                       select Name as Task, SUM(Cost) as TotalCosts
                                       from CostStats s
                                       join Tasks t on t.TaskId=s.TaskId
                                       group by s.TaskId
                                       order by 2 desc, 1 limit 5;
                                       """)
            .ToList();
    }

    public IEnumerable<EmployeeTotalTimeSpent> GetTop5Employees()
    {
        using var dbContext = CreateDbContext();

        return dbContext.Database
            .SqlQuery<EmployeeTotalTimeSpent>($"""
                                               with EmployeeTimeStats as (select EmployeeId, TIMESTAMPDIFF(HOUR, StartTime, EndTime) as HoursSpent from TimeSpent ts)
                                               select Name, SUM(HoursSpent) as TotalHoursSpent
                                               from EmployeeTimeStats s
                                               join Employees e on e.EmployeeId=s.EmployeeId
                                               group by s.EmployeeId
                                               order by 2 desc, 1 limit 5;
                                               """)
            .ToList();
    }

    private OutsourceTrackerContext CreateDbContext()
    {
        return new OutsourceTrackerContext(connectionString);
    }
}