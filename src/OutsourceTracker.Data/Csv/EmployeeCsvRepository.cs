namespace OutsourceTracker.Data.Csv;

public class EmployeeCsvRepository(string filePath) : CsvRepository<EmployeeDto, EmployeeMap>(filePath);