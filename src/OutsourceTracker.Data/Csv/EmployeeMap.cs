using CsvHelper.Configuration;

namespace OutsourceTracker.Data.Csv;

public sealed class EmployeeMap : ClassMap<EmployeeDto>
{
    public EmployeeMap()
    {
        Map(m => m.Name).Index(0);
        Map(m => m.Position).Index(1);
    }
}