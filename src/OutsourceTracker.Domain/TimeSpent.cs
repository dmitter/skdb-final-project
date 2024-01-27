namespace OutsourceTracker.Domain;

public class TimeSpent
{
    public uint TimeSpentId { get; set; }

    public uint TaskId { get; set; }

    public uint EmployeeId { get; set; }

    public required Task Task { get; set; }

    public required Employee Employee { get; set; }

    public required DateTime StartTime { get; set; }

    public required DateTime EndTime { get; set; }

    public bool ValidateTimeRange()
    {
        return StartTime < EndTime;
    }

    public bool IntersectWith(TimeSpent other)
    {
        return !(StartTime >= other.EndTime || EndTime <= other.StartTime);
    }
}