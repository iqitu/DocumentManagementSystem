using DocumentManagement.Core.Services;
using DocumentManagement.Models;
using Microsoft.Extensions.Logging;
using System.Text;

namespace DocumentManagement.Services;

public class TemplateConversionService : ITemplateConverter
{
    private readonly ILogger<TemplateConversionService> _logger;
    private readonly DataService _dataService;

    public TemplateConversionService(ILogger<TemplateConversionService> logger, DataService dataService)
    {
        _logger = logger;
        _dataService = dataService;
    }

    public bool IsFileTypeSupported(string extension)
    {
        var supportedExtensions = new[] { ".docx", ".doc", ".pdf", ".xlsx", ".xls", ".txt", ".rtf" };
        return supportedExtensions.Contains(extension.ToLowerInvariant());
    }

    public async Task<bool> ConvertToXamlAsync(DocumentTemplate template)
    {
        try
        {
            _logger.LogInformation("Starting conversion for template: {TemplateName}", template.Name);
            
            template.Status = TemplateStatus.Converting;
            
            // Simulate conversion delay
            await Task.Delay(2000);
            
            var xamlContent = await GenerateXamlContentAsync(template);
            
            var templatesDirectory = _dataService.GetTemplatesDirectory();
            var xamlFileName = $"{template.Id}.xaml";
            var xamlFilePath = Path.Combine(templatesDirectory, xamlFileName);
            
            await File.WriteAllTextAsync(xamlFilePath, xamlContent);
            
            template.XamlFilePath = xamlFilePath;
            template.Status = TemplateStatus.Converted;
            template.ConvertedDate = DateTime.Now;
            
            _logger.LogInformation("Template converted successfully: {TemplateName}", template.Name);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to convert template: {TemplateName}", template.Name);
            template.Status = TemplateStatus.ConversionFailed;
            return false;
        }
    }

    public async Task<string> GetXamlPreviewAsync(string filePath)
    {
        try
        {
            if (File.Exists(filePath))
            {
                return await File.ReadAllTextAsync(filePath);
            }
            return GenerateDefaultXaml();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to get XAML preview for: {FilePath}", filePath);
            return GenerateDefaultXaml();
        }
    }

    private async Task<string> GenerateXamlContentAsync(DocumentTemplate template)
    {
        var xamlBuilder = new StringBuilder();
        
        try
        {
            switch (template.FileType)
            {
                case FileType.Text:
                    xamlBuilder.Append(await GenerateTextXamlAsync(template));
                    break;
                case FileType.Word:
                    xamlBuilder.Append(GenerateWordXaml(template));
                    break;
                case FileType.PDF:
                    xamlBuilder.Append(GeneratePdfXaml(template));
                    break;
                case FileType.Excel:
                    xamlBuilder.Append(GenerateExcelXaml(template));
                    break;
                default:
                    xamlBuilder.Append(GenerateDefaultXaml());
                    break;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error generating XAML content for template: {TemplateName}", template.Name);
            xamlBuilder.Clear();
            xamlBuilder.Append(GenerateDefaultXaml());
        }
        
        return xamlBuilder.ToString();
    }

    private async Task<string> GenerateTextXamlAsync(DocumentTemplate template)
    {
        try
        {
            var content = await File.ReadAllTextAsync(template.OriginalFilePath);
            var escapedContent = System.Security.SecurityElement.Escape(content);
            
            return $@"<UserControl x:Class=""DocumentTemplate_{template.Id}""
                      xmlns=""http://schemas.microsoft.com/winfx/2006/xaml/presentation""
                      xmlns:x=""http://schemas.microsoft.com/winfx/2006/xaml"">
    <Grid>
        <Border Background=""White"" BorderBrush=""LightGray"" BorderThickness=""1"" Margin=""10"" Padding=""15"">
            <ScrollViewer VerticalScrollBarVisibility=""Auto"">
                <TextBlock Text=""{escapedContent}"" 
                          TextWrapping=""Wrap"" 
                          FontFamily=""Segoe UI"" 
                          FontSize=""12"" 
                          LineHeight=""18""/>
            </ScrollViewer>
        </Border>
    </Grid>
</UserControl>";
        }
        catch
        {
            return GenerateDefaultXaml();
        }
    }

    private string GenerateWordXaml(DocumentTemplate template)
    {
        return $@"<UserControl x:Class=""DocumentTemplate_{template.Id}""
                  xmlns=""http://schemas.microsoft.com/winfx/2006/xaml/presentation""
                  xmlns:x=""http://schemas.microsoft.com/winfx/2006/xaml"">
    <Grid>
        <Border Background=""White"" BorderBrush=""LightBlue"" BorderThickness=""2"" Margin=""10"" Padding=""20"">
            <StackPanel>
                <TextBlock Text=""Word Document Template"" FontWeight=""Bold"" FontSize=""16"" Margin=""0,0,0,10""/>
                <TextBlock Text=""Template: {System.Security.SecurityElement.Escape(template.Name)}"" FontSize=""14"" Margin=""0,0,0,5""/>
                <TextBlock Text=""Category: {System.Security.SecurityElement.Escape(template.Category)}"" FontSize=""12"" Margin=""0,0,0,5""/>
                <TextBlock Text=""Description: {System.Security.SecurityElement.Escape(template.Description)}"" FontSize=""12"" Margin=""0,0,0,10"" TextWrapping=""Wrap""/>
                <Rectangle Fill=""LightGray"" Height=""200"" Margin=""0,10""/>
                <TextBlock Text=""[Word document content would be rendered here]"" 
                          HorizontalAlignment=""Center"" 
                          VerticalAlignment=""Center"" 
                          FontStyle=""Italic"" 
                          Foreground=""Gray""/>
            </StackPanel>
        </Border>
    </Grid>
</UserControl>";
    }

    private string GeneratePdfXaml(DocumentTemplate template)
    {
        return $@"<UserControl x:Class=""DocumentTemplate_{template.Id}""
                  xmlns=""http://schemas.microsoft.com/winfx/2006/xaml/presentation""
                  xmlns:x=""http://schemas.microsoft.com/winfx/2006/xaml"">
    <Grid>
        <Border Background=""White"" BorderBrush=""Red"" BorderThickness=""2"" Margin=""10"" Padding=""20"">
            <StackPanel>
                <TextBlock Text=""PDF Document Template"" FontWeight=""Bold"" FontSize=""16"" Margin=""0,0,0,10""/>
                <TextBlock Text=""Template: {System.Security.SecurityElement.Escape(template.Name)}"" FontSize=""14"" Margin=""0,0,0,5""/>
                <TextBlock Text=""Category: {System.Security.SecurityElement.Escape(template.Category)}"" FontSize=""12"" Margin=""0,0,0,5""/>
                <TextBlock Text=""Description: {System.Security.SecurityElement.Escape(template.Description)}"" FontSize=""12"" Margin=""0,0,0,10"" TextWrapping=""Wrap""/>
                <Rectangle Fill=""LightCoral"" Height=""200"" Margin=""0,10""/>
                <TextBlock Text=""[PDF document content would be rendered here]"" 
                          HorizontalAlignment=""Center"" 
                          VerticalAlignment=""Center"" 
                          FontStyle=""Italic"" 
                          Foreground=""Gray""/>
            </StackPanel>
        </Border>
    </Grid>
</UserControl>";
    }

    private string GenerateExcelXaml(DocumentTemplate template)
    {
        return $@"<UserControl x:Class=""DocumentTemplate_{template.Id}""
                  xmlns=""http://schemas.microsoft.com/winfx/2006/xaml/presentation""
                  xmlns:x=""http://schemas.microsoft.com/winfx/2006/xaml"">
    <Grid>
        <Border Background=""White"" BorderBrush=""Green"" BorderThickness=""2"" Margin=""10"" Padding=""20"">
            <StackPanel>
                <TextBlock Text=""Excel Document Template"" FontWeight=""Bold"" FontSize=""16"" Margin=""0,0,0,10""/>
                <TextBlock Text=""Template: {System.Security.SecurityElement.Escape(template.Name)}"" FontSize=""14"" Margin=""0,0,0,5""/>
                <TextBlock Text=""Category: {System.Security.SecurityElement.Escape(template.Category)}"" FontSize=""12"" Margin=""0,0,0,5""/>
                <TextBlock Text=""Description: {System.Security.SecurityElement.Escape(template.Description)}"" FontSize=""12"" Margin=""0,0,0,10"" TextWrapping=""Wrap""/>
                <Grid>
                    <Grid.RowDefinitions>
                        <RowDefinition Height=""30""/>
                        <RowDefinition Height=""30""/>
                        <RowDefinition Height=""30""/>
                    </Grid.RowDefinitions>
                    <Grid.ColumnDefinitions>
                        <ColumnDefinition Width=""*""/>
                        <ColumnDefinition Width=""*""/>
                        <ColumnDefinition Width=""*""/>
                    </Grid.ColumnDefinitions>
                    <Border Grid.Row=""0"" Grid.Column=""0"" BorderBrush=""Gray"" BorderThickness=""1"">
                        <TextBlock Text=""A1"" HorizontalAlignment=""Center"" VerticalAlignment=""Center""/>
                    </Border>
                    <Border Grid.Row=""0"" Grid.Column=""1"" BorderBrush=""Gray"" BorderThickness=""1"">
                        <TextBlock Text=""B1"" HorizontalAlignment=""Center"" VerticalAlignment=""Center""/>
                    </Border>
                    <Border Grid.Row=""0"" Grid.Column=""2"" BorderBrush=""Gray"" BorderThickness=""1"">
                        <TextBlock Text=""C1"" HorizontalAlignment=""Center"" VerticalAlignment=""Center""/>
                    </Border>
                    <Border Grid.Row=""1"" Grid.Column=""0"" BorderBrush=""Gray"" BorderThickness=""1"">
                        <TextBlock Text=""A2"" HorizontalAlignment=""Center"" VerticalAlignment=""Center""/>
                    </Border>
                    <Border Grid.Row=""1"" Grid.Column=""1"" BorderBrush=""Gray"" BorderThickness=""1"">
                        <TextBlock Text=""B2"" HorizontalAlignment=""Center"" VerticalAlignment=""Center""/>
                    </Border>
                    <Border Grid.Row=""1"" Grid.Column=""2"" BorderBrush=""Gray"" BorderThickness=""1"">
                        <TextBlock Text=""C2"" HorizontalAlignment=""Center"" VerticalAlignment=""Center""/>
                    </Border>
                </Grid>
                <TextBlock Text=""[Excel spreadsheet content would be rendered here]"" 
                          HorizontalAlignment=""Center"" 
                          Margin=""0,10"" 
                          FontStyle=""Italic"" 
                          Foreground=""Gray""/>
            </StackPanel>
        </Border>
    </Grid>
</UserControl>";
    }

    private string GenerateDefaultXaml()
    {
        return @"<UserControl xmlns=""http://schemas.microsoft.com/winfx/2006/xaml/presentation""
                 xmlns:x=""http://schemas.microsoft.com/winfx/2006/xaml"">
    <Grid>
        <Border Background=""White"" BorderBrush=""Gray"" BorderThickness=""1"" Margin=""10"" Padding=""20"">
            <StackPanel HorizontalAlignment=""Center"" VerticalAlignment=""Center"">
                <TextBlock Text=""Document Template"" FontWeight=""Bold"" FontSize=""16"" Margin=""0,0,0,10""/>
                <TextBlock Text=""Template preview not available"" FontStyle=""Italic"" Foreground=""Gray""/>
            </StackPanel>
        </Border>
    </Grid>
</UserControl>";
    }
}