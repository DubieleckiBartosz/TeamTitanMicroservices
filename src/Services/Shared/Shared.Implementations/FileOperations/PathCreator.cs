namespace Shared.Implementations.FileOperations;

public static class PathCreator
{
    public static string CreatePath(this string rootPath, string name, string fileName)
    {
        var path = $"{name ?? "File"}-{DateTime.Now.ToShortDateString()}-{fileName}";
        return Path.Combine(rootPath, path);
    }
}