namespace OutsourceTracker.Domain;

public class Employee
{
    public uint EmployeeId { get; set; }

    public required string Name { get; set; }

    public required Position Position { get; set; }

    public List<TimeSpent> TimeSheet { get; } = [];
}