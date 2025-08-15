using DocumentManagement.Models;
using DocumentManagement.Services;
using System;
using System.IO;
using System.Threading.Tasks;

namespace DocumentManagementSystem.Dialogs;

/// <summary>
/// Dialog for uploading new templates
/// In WPF, this would be a Window with XAML UI
/// </summary>
public class TemplateUploadDialog
{
    private readonly FileUploadService _fileUploadService;
    private readonly TemplateConversionService _templateConversionService;

    public TemplateUploadDialog(FileUploadService fileUploadService, TemplateConversionService templateConversionService)
    {
        _fileUploadService = fileUploadService;
        _templateConversionService = templateConversionService;
    }

    public async Task<DocumentTemplate?> ShowDialogAsync()
    {
        Console.WriteLine("Template Upload Dialog");
        Console.WriteLine("=====================");
        
        Console.Write("Template Name: ");
        var name = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(name))
            return null;

        Console.Write("Category: ");
        var category = Console.ReadLine() ?? "General";

        Console.Write("Description: ");
        var description = Console.ReadLine() ?? "";

        Console.Write("File Path: ");
        var filePath = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(filePath) || !File.Exists(filePath))
        {
            Console.WriteLine("Invalid file path.");
            return null;
        }

        if (!_fileUploadService.IsFileTypeSupported(filePath))
        {
            Console.WriteLine("File type not supported.");
            return null;
        }

        var fileName = Path.GetFileName(filePath);
        var uploadedPath = await _fileUploadService.UploadFileAsync(filePath, fileName);
        
        if (uploadedPath == null)
        {
            Console.WriteLine("Failed to upload file.");
            return null;
        }

        var template = new DocumentTemplate
        {
            Name = name,
            Category = category,
            Description = description,
            OriginalFilePath = uploadedPath,
            FileType = _fileUploadService.GetFileType(filePath),
            FileSize = _fileUploadService.GetFileSize(uploadedPath),
            Status = TemplateStatus.Uploaded
        };

        Console.WriteLine("Template uploaded successfully!");
        Console.Write("Convert to XAML now? (y/n): ");
        var convert = Console.ReadLine()?.ToLowerInvariant() == "y";
        
        if (convert)
        {
            Console.WriteLine("Converting template...");
            await _templateConversionService.ConvertToXamlAsync(template);
            Console.WriteLine($"Conversion status: {template.Status}");
        }

        return template;
    }
}

/* Example XAML for WPF version:
<Window x:Class="DocumentManagementSystem.Dialogs.TemplateUploadDialog"
        xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
        xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
        Title="Upload Template" Height="400" Width="500"
        WindowStartupLocation="CenterParent">
    <Grid Margin="20">
        <Grid.RowDefinitions>
            <RowDefinition Height="Auto"/>
            <RowDefinition Height="Auto"/>
            <RowDefinition Height="Auto"/>
            <RowDefinition Height="Auto"/>
            <RowDefinition Height="Auto"/>
            <RowDefinition Height="*"/>
            <RowDefinition Height="Auto"/>
        </Grid.RowDefinitions>
        
        <TextBlock Grid.Row="0" Text="Upload New Template" FontSize="18" FontWeight="Bold" Margin="0,0,0,20"/>
        
        <StackPanel Grid.Row="1" Orientation="Horizontal" Margin="0,5">
            <TextBlock Text="Name:" Width="80" VerticalAlignment="Center"/>
            <TextBox x:Name="NameTextBox" Width="300"/>
        </StackPanel>
        
        <StackPanel Grid.Row="2" Orientation="Horizontal" Margin="0,5">
            <TextBlock Text="Category:" Width="80" VerticalAlignment="Center"/>
            <ComboBox x:Name="CategoryComboBox" Width="300" IsEditable="True"/>
        </StackPanel>
        
        <StackPanel Grid.Row="3" Orientation="Horizontal" Margin="0,5">
            <TextBlock Text="File:" Width="80" VerticalAlignment="Center"/>
            <TextBox x:Name="FilePathTextBox" Width="250"/>
            <Button x:Name="BrowseButton" Content="Browse..." Width="70" Margin="5,0,0,0"/>
        </StackPanel>
        
        <StackPanel Grid.Row="4" Orientation="Horizontal" Margin="0,5">
            <TextBlock Text="Description:" Width="80" VerticalAlignment="Top" Margin="0,5,0,0"/>
            <TextBox x:Name="DescriptionTextBox" Width="300" Height="60" TextWrapping="Wrap" AcceptsReturn="True"/>
        </StackPanel>
        
        <CheckBox Grid.Row="5" x:Name="ConvertImmediatelyCheckBox" Content="Convert to XAML immediately" 
                  VerticalAlignment="Top" Margin="80,10,0,0"/>
        
        <StackPanel Grid.Row="6" Orientation="Horizontal" HorizontalAlignment="Right" Margin="0,20,0,0">
            <Button x:Name="UploadButton" Content="Upload" Width="80" Height="30" Margin="0,0,10,0"/>
            <Button x:Name="CancelButton" Content="Cancel" Width="80" Height="30"/>
        </StackPanel>
    </Grid>
</Window>
*/