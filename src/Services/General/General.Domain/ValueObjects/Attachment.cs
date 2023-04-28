using Shared.Domain.Base;

namespace General.Domain.ValueObjects;

public class Attachment : ValueObject
{
    public string Title { get; private set; }
    public string Path { get; private set; }

    private Attachment(string title, string path)
    {
        Title = title;
        Path = path;
    }

    public static Attachment Create(string title, string path) => new Attachment(title, path);
    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Title;
        yield return Path;
    }
}