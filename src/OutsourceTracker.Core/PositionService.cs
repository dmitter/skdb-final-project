using OutsourceTracker.Data;
using OutsourceTracker.Data.Csv;
using OutsourceTracker.Domain;

namespace OutsourceTracker.Core;

public class PositionService(string connectionString) : IImportService
{
    public IEnumerable<OperationResult> Import(string filePath)
    {
        using var csvRepository = new PositionsCsvRepository(filePath);
        foreach (var position in csvRepository.GetItems()) yield return ImportPosition(position);
    }

    private OperationResult ImportPosition(Position position)
    {
        using var dbContext = new OutsourceTrackerContext(connectionString);
        dbContext.Positions.Add(position);
        try
        {
            dbContext.SaveChanges();
            return OperationResult.Success();
        }
        catch (Exception ex)
        {
            return OperationResult.Error(ex);
        }
    }
}