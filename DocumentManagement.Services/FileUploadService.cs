using DocumentManagement.Models;
using Microsoft.Extensions.Logging;

namespace DocumentManagement.Services;

public class FileUploadService
{
    private readonly ILogger<FileUploadService> _logger;
    private readonly DataService _dataService;
    private readonly HashSet<string> _supportedExtensions = new(StringComparer.OrdinalIgnoreCase)
    {
        ".docx", ".doc", ".pdf", ".xlsx", ".xls", ".txt", ".rtf"
    };

    public FileUploadService(ILogger<FileUploadService> logger, DataService dataService)
    {
        _logger = logger;
        _dataService = dataService;
    }

    public bool IsFileTypeSupported(string filePath)
    {
        var extension = Path.GetExtension(filePath);
        return _supportedExtensions.Contains(extension);
    }

    public FileType GetFileType(string filePath)
    {
        var extension = Path.GetExtension(filePath).ToLowerInvariant();
        return extension switch
        {
            ".docx" or ".doc" => FileType.Word,
            ".xlsx" or ".xls" => FileType.Excel,
            ".pdf" => FileType.PDF,
            ".txt" or ".rtf" => FileType.Text,
            ".xaml" => FileType.XAML,
            _ => FileType.Unknown
        };
    }

    public async Task<string?> UploadFileAsync(string sourcePath, string fileName)
    {
        try
        {
            if (!File.Exists(sourcePath))
            {
                _logger.LogError("Source file does not exist: {SourcePath}", sourcePath);
                return null;
            }

            if (!IsFileTypeSupported(sourcePath))
            {
                _logger.LogError("File type not supported: {SourcePath}", sourcePath);
                return null;
            }

            var uploadsDirectory = _dataService.GetUploadsDirectory();
            var uniqueFileName = GenerateUniqueFileName(fileName);
            var destinationPath = Path.Combine(uploadsDirectory, uniqueFileName);

            using var sourceStream = File.OpenRead(sourcePath);
            using var destinationStream = File.Create(destinationPath);
            await sourceStream.CopyToAsync(destinationStream);
            
            _logger.LogInformation("File uploaded successfully: {FileName}", uniqueFileName);
            return destinationPath;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to upload file: {SourcePath}", sourcePath);
            return null;
        }
    }

    public Task<bool> DeleteFileAsync(string filePath)
    {
        try
        {
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
                _logger.LogInformation("File deleted: {FilePath}", filePath);
                return Task.FromResult(true);
            }
            return Task.FromResult(false);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to delete file: {FilePath}", filePath);
            return Task.FromResult(false);
        }
    }

    public long GetFileSize(string filePath)
    {
        try
        {
            if (File.Exists(filePath))
            {
                return new FileInfo(filePath).Length;
            }
            return 0;
        }
        catch
        {
            return 0;
        }
    }

    private string GenerateUniqueFileName(string originalFileName)
    {
        var name = Path.GetFileNameWithoutExtension(originalFileName);
        var extension = Path.GetExtension(originalFileName);
        var timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
        var randomSuffix = Path.GetRandomFileName()[..8];
        
        return $"{name}_{timestamp}_{randomSuffix}{extension}";
    }

    public string FormatFileSize(long bytes)
    {
        string[] sizes = { "B", "KB", "MB", "GB", "TB" };
        double len = bytes;
        int order = 0;
        while (len >= 1024 && order < sizes.Length - 1)
        {
            order++;
            len = len / 1024;
        }
        return $"{len:0.##} {sizes[order]}";
    }
}