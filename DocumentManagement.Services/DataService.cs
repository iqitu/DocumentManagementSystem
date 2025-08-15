using DocumentManagement.Models;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace DocumentManagement.Services;

public class DataService
{
    private readonly ILogger<DataService> _logger;
    private readonly string _dataDirectory;
    private readonly string _templatesFile;
    private readonly string _documentsFile;
    private readonly string _backupDirectory;

    public DataService(ILogger<DataService> logger)
    {
        _logger = logger;
        _dataDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "DocumentManagementSystem");
        _templatesFile = Path.Combine(_dataDirectory, "templates.json");
        _documentsFile = Path.Combine(_dataDirectory, "documents.json");
        _backupDirectory = Path.Combine(_dataDirectory, "Backups");
        
        EnsureDirectoriesExist();
    }

    private void EnsureDirectoriesExist()
    {
        try
        {
            Directory.CreateDirectory(_dataDirectory);
            Directory.CreateDirectory(_backupDirectory);
            Directory.CreateDirectory(Path.Combine(_dataDirectory, "Templates"));
            Directory.CreateDirectory(Path.Combine(_dataDirectory, "Uploads"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to create data directories");
        }
    }

    public async Task<List<DocumentTemplate>> GetTemplatesAsync()
    {
        try
        {
            if (!File.Exists(_templatesFile))
                return new List<DocumentTemplate>();

            var json = await File.ReadAllTextAsync(_templatesFile);
            return JsonConvert.DeserializeObject<List<DocumentTemplate>>(json) ?? new List<DocumentTemplate>();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to load templates");
            return new List<DocumentTemplate>();
        }
    }

    public async Task<bool> SaveTemplatesAsync(List<DocumentTemplate> templates)
    {
        try
        {
            await CreateBackupAsync(_templatesFile);
            var json = JsonConvert.SerializeObject(templates, Formatting.Indented);
            await File.WriteAllTextAsync(_templatesFile, json);
            _logger.LogInformation("Templates saved successfully");
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to save templates");
            return false;
        }
    }

    public async Task<List<IncomingDocument>> GetDocumentsAsync()
    {
        try
        {
            if (!File.Exists(_documentsFile))
                return new List<IncomingDocument>();

            var json = await File.ReadAllTextAsync(_documentsFile);
            return JsonConvert.DeserializeObject<List<IncomingDocument>>(json) ?? new List<IncomingDocument>();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to load documents");
            return new List<IncomingDocument>();
        }
    }

    public async Task<bool> SaveDocumentsAsync(List<IncomingDocument> documents)
    {
        try
        {
            await CreateBackupAsync(_documentsFile);
            var json = JsonConvert.SerializeObject(documents, Formatting.Indented);
            await File.WriteAllTextAsync(_documentsFile, json);
            _logger.LogInformation("Documents saved successfully");
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to save documents");
            return false;
        }
    }

    private async Task CreateBackupAsync(string filePath)
    {
        if (!File.Exists(filePath))
            return;

        try
        {
            var fileName = Path.GetFileName(filePath);
            var backupFileName = $"{fileName}.{DateTime.Now:yyyyMMdd_HHmmss}.bak";
            var backupPath = Path.Combine(_backupDirectory, backupFileName);
            
            using var sourceStream = File.OpenRead(filePath);
            using var destinationStream = File.Create(backupPath);
            await sourceStream.CopyToAsync(destinationStream);
            
            // Keep only last 10 backups
            var backups = Directory.GetFiles(_backupDirectory, $"{fileName}.*.bak")
                .OrderByDescending(f => f)
                .Skip(10);
            
            foreach (var oldBackup in backups)
            {
                File.Delete(oldBackup);
            }
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Failed to create backup for {FilePath}", filePath);
        }
    }

    public string GetDataDirectory() => _dataDirectory;
    public string GetTemplatesDirectory() => Path.Combine(_dataDirectory, "Templates");
    public string GetUploadsDirectory() => Path.Combine(_dataDirectory, "Uploads");
}
