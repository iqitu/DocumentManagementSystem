using DocumentManagement.Models;

namespace DocumentManagement.Core.Services;

public interface ITemplateConverter
{
    Task<bool> ConvertToXamlAsync(DocumentTemplate template);
    Task<string> GetXamlPreviewAsync(string filePath);
    bool IsFileTypeSupported(string extension);
}
