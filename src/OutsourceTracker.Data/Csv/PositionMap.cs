using CsvHelper.Configuration;
using OutsourceTracker.Domain;

namespace OutsourceTracker.Data.Csv;

public sealed class PositionMap : ClassMap<Position>
{
    public PositionMap()
    {
        Map(m => m.Name).Index(0);
        Map(m => m.HourlyRate).Index(1);
    }
}