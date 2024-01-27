namespace OutsourceTracker.Data.Csv;

public record EmployeeDto
{
    public required string Name { get; init; }

    public required string Position { get; init; }
}