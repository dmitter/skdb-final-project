namespace OutsourceTracker.Data.Csv;

public interface ITimesheetEntryDto
{
    string Employee { get; init; }
    DateTime EndTime { get; init; }
    DateTime StartTime { get; init; }
    string Task { get; init; }

    void Deconstruct(out string Task, out string Employee, out DateTime StartTime, out DateTime EndTime);
    bool Equals(object? obj);
    bool Equals(TimesheetEntryDto? other);
    int GetHashCode();
    string ToString();
}