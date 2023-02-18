namespace Shared.Implementations.Communications.Email;

public class EmailDetails
{
    public List<string> Recipients { get; set; }
    public string Subject { get; set; }
    public string Body { get; set; }
    public string FromName { get; set; }
}