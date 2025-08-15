using DocumentManagement.Models;
using DocumentManagement.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using System;
using System.Threading.Tasks;
using System.IO;
using System.Collections.Generic;
using System.Linq;

namespace DocumentManagementSystem;

class Program
{
    static async Task Main(string[] args)
    {
        Console.WriteLine("Document Management System");
        Console.WriteLine("==========================");
        
        // Setup services
        var services = new ServiceCollection();
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: true)
            .Build();
        
        services.AddSingleton<IConfiguration>(configuration);
        services.AddLogging(builder => builder.AddConsole());
        services.AddSingleton<DataService>();
        services.AddSingleton<FileUploadService>();
        services.AddSingleton<TemplateConversionService>();
        
        var serviceProvider = services.BuildServiceProvider();
        
        // Initialize application
        var app = new DocumentManagementApp(serviceProvider);
        await app.RunAsync();
    }
}

public class DocumentManagementApp
{
    private readonly IServiceProvider _serviceProvider;
    private readonly DataService _dataService;
    private readonly FileUploadService _fileUploadService;
    private readonly TemplateConversionService _templateConversionService;
    private readonly ILogger<DocumentManagementApp> _logger;

    public DocumentManagementApp(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        _dataService = serviceProvider.GetRequiredService<DataService>();
        _fileUploadService = serviceProvider.GetRequiredService<FileUploadService>();
        _templateConversionService = serviceProvider.GetRequiredService<TemplateConversionService>();
        _logger = serviceProvider.GetRequiredService<ILogger<DocumentManagementApp>>();
    }

    public async Task RunAsync()
    {
        _logger.LogInformation("Document Management System starting...");
        
        while (true)
        {
            ShowMainMenu();
            var choice = Console.ReadLine();
            
            try
            {
                switch (choice)
                {
                    case "1":
                        await ShowDashboard();
                        break;
                    case "2":
                        await ManageTemplates();
                        break;
                    case "3":
                        await ManageDocuments();
                        break;
                    case "4":
                        await CreateSampleData();
                        break;
                    case "0":
                        return;
                    default:
                        Console.WriteLine("Invalid choice. Please try again.");
                        break;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing menu choice: {Choice}", choice);
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
            
            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
        }
    }

    private void ShowMainMenu()
    {
        Console.Clear();
        Console.WriteLine("Document Management System");
        Console.WriteLine("==========================");
        Console.WriteLine("1. Dashboard");
        Console.WriteLine("2. Manage Templates");
        Console.WriteLine("3. Manage Documents");
        Console.WriteLine("4. Create Sample Data");
        Console.WriteLine("0. Exit");
        Console.WriteLine();
        Console.Write("Select an option: ");
    }

    private async Task ShowDashboard()
    {
        Console.Clear();
        Console.WriteLine("Dashboard");
        Console.WriteLine("=========");
        
        var templates = await _dataService.GetTemplatesAsync();
        var documents = await _dataService.GetDocumentsAsync();
        
        Console.WriteLine($"Total Templates: {templates.Count}");
        Console.WriteLine($"  - Active: {templates.Count(t => t.Status == TemplateStatus.Active)}");
        Console.WriteLine($"  - Converting: {templates.Count(t => t.Status == TemplateStatus.Converting)}");
        Console.WriteLine($"  - Converted: {templates.Count(t => t.Status == TemplateStatus.Converted)}");
        Console.WriteLine($"  - Failed: {templates.Count(t => t.Status == TemplateStatus.ConversionFailed)}");
        Console.WriteLine();
        
        Console.WriteLine($"Total Documents: {documents.Count}");
        Console.WriteLine($"  - Pending: {documents.Count(d => d.Status == DocumentStatus.Pending)}");
        Console.WriteLine($"  - In Progress: {documents.Count(d => d.Status == DocumentStatus.InProgress)}");
        Console.WriteLine($"  - Completed: {documents.Count(d => d.Status == DocumentStatus.Completed)}");
        Console.WriteLine($"  - Rejected: {documents.Count(d => d.Status == DocumentStatus.Rejected)}");
        Console.WriteLine($"  - Archived: {documents.Count(d => d.Status == DocumentStatus.Archived)}");
        Console.WriteLine();
        
        Console.WriteLine("Priority Distribution:");
        Console.WriteLine($"  - Urgent: {documents.Count(d => d.Priority == DocumentPriority.Urgent)}");
        Console.WriteLine($"  - High: {documents.Count(d => d.Priority == DocumentPriority.High)}");
        Console.WriteLine($"  - Normal: {documents.Count(d => d.Priority == DocumentPriority.Normal)}");
        Console.WriteLine($"  - Low: {documents.Count(d => d.Priority == DocumentPriority.Low)}");
    }

    private async Task ManageTemplates()
    {
        Console.Clear();
        Console.WriteLine("Template Management");
        Console.WriteLine("==================");
        
        var templates = await _dataService.GetTemplatesAsync();
        
        if (templates.Count == 0)
        {
            Console.WriteLine("No templates found.");
            return;
        }
        
        Console.WriteLine("Available Templates:");
        Console.WriteLine("ID".PadRight(38) + "Name".PadRight(25) + "Status".PadRight(15) + "Type");
        Console.WriteLine(new string('-', 80));
        
        foreach (var template in templates)
        {
            Console.WriteLine($"{template.Id.PadRight(38)}{template.Name.PadRight(25)}{template.Status.ToString().PadRight(15)}{template.FileType}");
        }
    }

    private async Task ManageDocuments()
    {
        Console.Clear();
        Console.WriteLine("Document Management");
        Console.WriteLine("==================");
        
        var documents = await _dataService.GetDocumentsAsync();
        
        if (documents.Count == 0)
        {
            Console.WriteLine("No documents found.");
            return;
        }
        
        Console.WriteLine("Available Documents:");
        Console.WriteLine("Title".PadRight(25) + "Status".PadRight(15) + "Priority".PadRight(10) + "Received");
        Console.WriteLine(new string('-', 75));
        
        foreach (var document in documents)
        {
            Console.WriteLine($"{document.Title.PadRight(25)}{document.Status.ToString().PadRight(15)}{document.Priority.ToString().PadRight(10)}{document.ReceivedDate:yyyy-MM-dd}");
        }
    }

    private async Task CreateSampleData()
    {
        Console.Clear();
        Console.WriteLine("Creating Sample Data");
        Console.WriteLine("====================");
        
        // Create sample templates
        var templates = new List<DocumentTemplate>
        {
            new DocumentTemplate
            {
                Name = "Invoice Template",
                Category = "Financial",
                Description = "Standard invoice template for billing",
                FileType = FileType.Word,
                Status = TemplateStatus.Active,
                FileSize = 1024 * 50, // 50KB
                OriginalFilePath = "sample_invoice.docx"
            },
            new DocumentTemplate
            {
                Name = "Report Template",
                Category = "Reports",
                Description = "Monthly report template",
                FileType = FileType.Excel,
                Status = TemplateStatus.Converted,
                FileSize = 1024 * 30, // 30KB
                OriginalFilePath = "sample_report.xlsx"
            },
            new DocumentTemplate
            {
                Name = "Contract Template",
                Category = "Legal",
                Description = "Standard contract template",
                FileType = FileType.PDF,
                Status = TemplateStatus.Converting,
                FileSize = 1024 * 100, // 100KB
                OriginalFilePath = "sample_contract.pdf"
            }
        };

        // Create sample documents
        var documents = new List<IncomingDocument>
        {
            new IncomingDocument
            {
                Title = "Purchase Order #12345",
                DocumentNumber = "PO-12345",
                Sender = "John Smith",
                Organization = "ABC Corp",
                Status = DocumentStatus.Pending,
                Priority = DocumentPriority.High,
                Notes = "Urgent processing required"
            },
            new IncomingDocument
            {
                Title = "Monthly Sales Report",
                DocumentNumber = "MSR-2024-01",
                Sender = "Jane Doe",
                Organization = "Sales Department",
                Status = DocumentStatus.InProgress,
                Priority = DocumentPriority.Normal,
                Notes = "Regular monthly reporting"
            },
            new IncomingDocument
            {
                Title = "Service Agreement",
                DocumentNumber = "SA-2024-001",
                Sender = "Mike Johnson",
                Organization = "Legal Department",
                Status = DocumentStatus.Completed,
                Priority = DocumentPriority.Low,
                Notes = "Service agreement processed and approved"
            }
        };

        await _dataService.SaveTemplatesAsync(templates);
        await _dataService.SaveDocumentsAsync(documents);
        
        Console.WriteLine($"Created {templates.Count} sample templates");
        Console.WriteLine($"Created {documents.Count} sample documents");
        Console.WriteLine("Sample data creation completed!");
    }
}