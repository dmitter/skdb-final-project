namespace OutsourceTracker.Data.Csv;

public class TimesheetCsvRepository(string filePath) : CsvRepository<TimesheetEntryDto, TimesheetEntryMap>(filePath);