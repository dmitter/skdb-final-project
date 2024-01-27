using OutsourceTracker.Domain;

namespace OutsourceTracker.Data.Csv;

public class PositionsCsvRepository(string filePath) : CsvRepository<Position, PositionMap>(filePath);