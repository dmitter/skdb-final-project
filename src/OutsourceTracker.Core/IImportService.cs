namespace OutsourceTracker.Core;

public interface IImportService
{
    IEnumerable<OperationResult> Import(string filePath);
}