namespace OutsourceTracker.Domain;

public class Position
{
    public uint PositionId { get; set; }

    public required string Name { get; set; }

    public required int HourlyRate { get; set; }
}