using Microsoft.AspNetCore.Http;
using Shared.Implementations.Logging;
using Shared.Implementations.Tools;

namespace Shared.Implementations.FileOperations;

public class FileService : IFileService
{
    private readonly ILoggerManager<FileService> _loggerManager;

    public FileService(ILoggerManager<FileService> loggerManager)
    {
        _loggerManager = loggerManager ?? throw new ArgumentNullException(nameof(loggerManager));
    }

    public async Task<string> SaveFileAsync(IFormFile formFile, string rootPath, string? name = null)
    {
        var rootFilePath = rootPath;
        if (!Directory.Exists(rootFilePath))
        {
            Directory.CreateDirectory(rootFilePath);
        }

        var path = name ?? formFile.CreateFilePath();
        var finalPath = Path.Combine(rootPath, path);

        await using Stream fileStream = new FileStream(finalPath, FileMode.Create);
        await formFile.CopyToAsync(fileStream);
        this._loggerManager.LogInformation($"The file has been CREATED: {finalPath}");
        return finalPath;
    }

    public void RemoveFile(string filePath)
    {
        {
            if (File.Exists(filePath))
            {
                try
                {
                    File.Delete(filePath);
                    this._loggerManager.LogInformation($"The file has been DELETED: {filePath}");
                }
                catch
                {
                    this._loggerManager.LogError($"file deletion FAILED: {filePath}");
                    throw;
                }
            }
        }
    }
}