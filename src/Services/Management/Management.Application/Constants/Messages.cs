namespace Management.Application.Constants;

public class Messages
{
    public static string DataNotFoundMessage(string name) => $"{name} not found"; 
    public static string ListNullOrEmptyMessage(string name) => $"{name} list is null or empty";

    public const string NameExistsMessageException = "Company name already exists.";
}