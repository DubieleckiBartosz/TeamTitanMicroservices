using Microsoft.AspNetCore.Http;

namespace Shared.Implementations.FileOperations;

public interface IFileService
{
    Task<string> SaveFileAsync(IFormFile formFile, string rootPath, string? name = null);
    void RemoveFile(string filePath);
}