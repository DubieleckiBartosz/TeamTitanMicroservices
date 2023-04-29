namespace General.Application.Constants;

public class ExceptionDetails
{
    public static string DetailsNotFound(string methodName) => $"{methodName} returned null"; 
    public const string DetailsNoPermissions = "You are not authorized to perform this operation"; 
}