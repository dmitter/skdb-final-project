using CsvHelper.Configuration;

namespace OutsourceTracker.Data.Csv;

public sealed class TimesheetEntryMap : ClassMap<TimesheetEntryDto>
{
    public TimesheetEntryMap()
    {
        Map(m => m.Task).Index(0);
        Map(m => m.Employee).Index(1);
        Map(m => m.StartTime).Index(2);
        Map(m => m.EndTime).Index(3);
    }
}