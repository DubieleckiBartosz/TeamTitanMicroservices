using Microsoft.AspNetCore.Http;

namespace Shared.Implementations.Tools;

public static class PathCreator
{
    public static string Create(this IFormFile formFile)
    {
        var path = $"File-{DateTime.Now.ToShortDateString()}-{Guid.NewGuid()}-{formFile.FileName}";
        return path;
    }

    public static string CreateFilePath(this IFormFile formFile)
    {
        var fileName = Path.GetFileNameWithoutExtension(formFile.FileName);
        var uniqueId = Guid.NewGuid().ToString("N");
        var dateStr = DateTime.UtcNow.ToString("yyyy-MM-dd");

        var filePath = $"{dateStr}-{uniqueId}-{fileName}{Path.GetExtension(formFile.FileName)}";
        return filePath;
    }
}