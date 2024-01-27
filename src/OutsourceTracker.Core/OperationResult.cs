namespace OutsourceTracker.Core;

public class OperationResult
{
    public required bool IsSuccessful { get; init; }

    public required string ErrorDescription { get; init; }

    public static OperationResult Success()
    {
        return new OperationResult
        {
            IsSuccessful = true,
            ErrorDescription = string.Empty
        };
    }

    public static OperationResult Error(string description)
    {
        return new OperationResult
        {
            IsSuccessful = false,
            ErrorDescription = description
        };
    }

    public static OperationResult Error(string description, Exception ex)
    {
        var message = GetMessage(ex);
        return new OperationResult
        {
            IsSuccessful = false,
            ErrorDescription = description + message
        };
    }

    public static OperationResult Error(Exception ex)
    {
        var message = GetMessage(ex);
        return new OperationResult
        {
            IsSuccessful = false,
            ErrorDescription = message
        };
    }

    private static string GetMessage(Exception ex)
        => ex.InnerException is null ? ex.Message : ex.InnerException.Message;
}