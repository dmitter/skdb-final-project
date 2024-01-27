namespace OutsourceTracker.Data.Csv;

public record TimesheetEntryDto
{
    public required string Employee { get; init; }

    public required DateTime EndTime { get; init; }

    public required DateTime StartTime { get; init; }

    public required string Task { get; init; }
}