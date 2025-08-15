# Document Management System

A comprehensive document management system built with .NET 8, featuring template management, document workflow tracking, and automatic XAML conversion capabilities. This system provides a robust foundation for managing document templates and processing incoming documents through their complete lifecycle.

## 🏗️ Architecture Overview

The system follows a clean architecture pattern with clear separation of concerns:

```
DocumentManagementSystem/
├── DocumentManagementSystem.sln           # Main solution file
├── DocumentManagementSystem/               # Main console application (WPF-ready)
├── DocumentManagement.Models/             # Data models and enums
├── DocumentManagement.Core/               # Core business logic interfaces
├── DocumentManagement.Services/           # Services implementation
└── appsettings.json                       # Configuration file
```

### Project Structure Details

- **DocumentManagementSystem**: Main application with console interface (easily convertible to WPF)
- **DocumentManagement.Models**: Contains all data models, enums, and value objects
- **DocumentManagement.Core**: Defines core business interfaces and contracts
- **DocumentManagement.Services**: Implements all business services and data persistence

## ✨ Features

### 📄 Template Management System
- **Multi-format Support**: Upload Word (.docx/.doc), PDF, Excel (.xlsx/.xls), and text files
- **Automatic XAML Conversion**: Convert uploaded documents to WPF XAML templates
- **Template Preview**: Real-time XAML preview functionality
- **Status Tracking**: Complete template lifecycle management
  - Uploaded → Converting → Converted/ConversionFailed → Active/Inactive
- **Metadata Management**: Comprehensive template information tracking
  - Name, category, description
  - File paths (original and XAML)
  - Creation and modification dates
  - File size and type information

### 📋 Document Workflow Management
- **Complete Lifecycle Tracking**: Manage documents from receipt to completion
- **Status Management**: Track document progress through defined stages
  - Pending → InProgress → Completed/Rejected/Archived
- **Priority System**: Four-level priority management (Low, Normal, High, Urgent)
- **Template Association**: Link documents to specific templates
- **Rich Metadata**: Comprehensive document information
  - Title, document number, sender, organization
  - Received and completion dates
  - Processing notes and history
- **Processing History**: Complete audit trail of document changes

### 🎨 User Interface
- **Console Application**: Fully functional console interface for demonstration
- **WPF-Ready Architecture**: Designed for easy conversion to WPF application
- **Dashboard Analytics**: Statistical overview of templates and documents
- **Search and Filter**: (Framework ready for implementation)
- **Modern Design Patterns**: MVVM-ready architecture

### 💾 Data Management
- **JSON Storage**: Lightweight, human-readable data persistence
- **Automatic Backups**: Configurable backup system with retention policies
- **Data Validation**: Comprehensive input validation and error handling
- **Migration Ready**: Architecture supports database migration

### 🔧 Technical Features
- **Dependency Injection**: Built-in DI container with service registration
- **Configuration Management**: JSON-based configuration with appsettings.json
- **Comprehensive Logging**: Microsoft.Extensions.Logging integration
- **Error Handling**: Robust exception management and recovery
- **Async/Await**: Full asynchronous operation support
- **Type Safety**: Nullable reference types enabled

## 🚀 Getting Started

### Prerequisites
- .NET 8.0 SDK or later
- Windows, macOS, or Linux

### Installation

1. **Clone the repository**
   ```bash
   git clone https://github.com/iqitu/DocumentManagementSystem.git
   cd DocumentManagementSystem
   ```

2. **Build the solution**
   ```bash
   dotnet build
   ```

3. **Run the application**
   ```bash
   dotnet run --project DocumentManagementSystem
   ```

### First Run Setup
1. The application will automatically create necessary directories
2. Choose option "4" to create sample data for demonstration
3. Explore the dashboard, templates, and documents features

## 📱 Usage Guide

### Main Menu Options

1. **Dashboard** - View system statistics and overview
   - Template counts by status
   - Document counts by status and priority
   - Visual representation of system health

2. **Manage Templates** - Template operations
   - View all templates with status information
   - Upload new templates (framework ready)
   - Edit template metadata (framework ready)
   - Preview XAML content (framework ready)

3. **Manage Documents** - Document operations
   - View all documents with status and priority
   - Create new documents (framework ready)
   - Edit document details (framework ready)
   - Process documents through workflow (framework ready)

4. **Create Sample Data** - Generate test data
   - Creates example templates and documents
   - Useful for testing and demonstration

### Configuration

Edit `appsettings.json` to customize:

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information"
    }
  },
  "DocumentManagement": {
    "DataDirectory": "%APPDATA%\\DocumentManagementSystem",
    "MaxBackups": 10,
    "SupportedFileTypes": [".docx", ".doc", ".pdf", ".xlsx", ".xls", ".txt", ".rtf"]
  }
}
```

## 🏗️ Architecture Details

### Data Models

#### DocumentTemplate
```csharp
public class DocumentTemplate : INotifyPropertyChanged
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string Category { get; set; }
    public string Description { get; set; }
    public string OriginalFilePath { get; set; }
    public string XamlFilePath { get; set; }
    public TemplateStatus Status { get; set; }
    public FileType FileType { get; set; }
    public long FileSize { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime ModifiedDate { get; set; }
    public DateTime? ConvertedDate { get; set; }
}
```

#### IncomingDocument
```csharp
public class IncomingDocument : INotifyPropertyChanged
{
    public string Id { get; set; }
    public string Title { get; set; }
    public string DocumentNumber { get; set; }
    public string Sender { get; set; }
    public string Organization { get; set; }
    public DocumentStatus Status { get; set; }
    public DocumentPriority Priority { get; set; }
    public string Notes { get; set; }
    public string TemplateId { get; set; }
    public DateTime ReceivedDate { get; set; }
    public DateTime ModifiedDate { get; set; }
    public DateTime? CompletedDate { get; set; }
    public List<ProcessingHistoryEntry> ProcessingHistory { get; set; }
}
```

### Services Layer

#### DataService
- JSON-based data persistence
- Automatic backup management
- CRUD operations for templates and documents
- Directory management

#### FileUploadService
- Multi-format file support validation
- Secure file upload handling
- File size calculation and formatting
- Unique filename generation

#### TemplateConversionService
- Document to XAML conversion
- Format-specific conversion strategies
- Preview generation
- Conversion status management

### Value Converters

The system includes WPF-ready value converters:

- **FileSizeConverter**: Human-readable file size formatting
- **StatusConverter**: Status to color/text/icon mapping
- **BooleanConverter**: Boolean to various UI representations

## 🎨 WPF Migration Guide

This console application is designed for easy migration to WPF:

### Steps to Convert to WPF:

1. **Update Project File**
   ```xml
   <Project Sdk="Microsoft.NET.Sdk">
     <PropertyGroup>
       <OutputType>WinExe</OutputType>
       <TargetFramework>net8.0-windows</TargetFramework>
       <UseWPF>true</UseWPF>
     </PropertyGroup>
   </Project>
   ```

2. **Create XAML Files**
   - MainWindow.xaml with TabControl for Dashboard, Templates, Documents
   - Dialog XAML files (examples provided in source code comments)

3. **Implement ViewModels**
   - MainWindowViewModel
   - DashboardViewModel
   - TemplateManagementViewModel
   - DocumentManagementViewModel

4. **Add Value Converters**
   - The converter classes are already implemented
   - Register them as StaticResources in App.xaml

5. **Update Dependency Injection**
   - Register ViewModels in the DI container
   - Set up View-ViewModel relationships

## 📊 Sample Data

The system includes comprehensive sample data generation:

### Sample Templates
- **Invoice Template** (Word, Active)
- **Report Template** (Excel, Converted) 
- **Contract Template** (PDF, Converting)

### Sample Documents
- **Purchase Order** (Pending, High Priority)
- **Sales Report** (In Progress, Normal Priority)
- **Service Agreement** (Completed, Low Priority)

## 🔍 Technical Implementation Details

### Asynchronous Operations
All file operations and data persistence use async/await patterns for optimal performance.

### Error Handling
Comprehensive try-catch blocks with proper logging ensure system stability.

### Data Validation
Input validation at multiple layers prevents data corruption and ensures data integrity.

### Memory Management
Proper disposal of file streams and resources prevents memory leaks.

### Logging
Structured logging with different log levels for debugging and monitoring.

## 📁 File Structure

```
DocumentManagementSystem/
├── DocumentManagementSystem.sln
├── DocumentManagementSystem/
│   ├── DocumentManagementSystem.csproj
│   ├── Program.cs                          # Main application entry point
│   ├── Converters/
│   │   ├── FileSizeConverter.cs           # File size formatting
│   │   ├── StatusConverter.cs             # Status to UI mappings
│   │   └── BooleanConverter.cs            # Boolean value conversions
│   └── Dialogs/
│       ├── TemplateUploadDialog.cs        # Template upload logic
│       └── DocumentEditDialog.cs          # Document editing logic
├── DocumentManagement.Models/
│   ├── DocumentManagement.Models.csproj
│   ├── DocumentTemplate.cs                # Template data model
│   ├── IncomingDocument.cs               # Document data model
│   └── Enums.cs                          # Status and priority enums
├── DocumentManagement.Core/
│   ├── DocumentManagement.Core.csproj
│   └── Services/
│       └── ITemplateConverter.cs          # Template conversion interface
├── DocumentManagement.Services/
│   ├── DocumentManagement.Services.csproj
│   ├── DataService.cs                     # Data persistence service
│   ├── FileUploadService.cs              # File management service
│   └── TemplateConversionService.cs      # Template conversion service
├── appsettings.json                       # Application configuration
└── README.md                             # This file
```

## 🛠️ Development

### Building the Project
```bash
# Build all projects
dotnet build

# Build specific project
dotnet build DocumentManagementSystem

# Build in Release mode
dotnet build -c Release
```

### Running Tests
```bash
# Run all tests (when test projects are added)
dotnet test

# Run with coverage
dotnet test --collect:"XPlat Code Coverage"
```

### Adding New Features

1. **New Models**: Add to DocumentManagement.Models
2. **New Services**: Add interface to Core, implementation to Services
3. **New UI Components**: Add to main project with MVVM pattern
4. **New Converters**: Add to Converters folder

## 🤝 Contributing

1. Fork the repository
2. Create a feature branch (`git checkout -b feature/AmazingFeature`)
3. Commit your changes (`git commit -m 'Add some AmazingFeature'`)
4. Push to the branch (`git push origin feature/AmazingFeature`)
5. Open a Pull Request

## 📝 License

This project is licensed under the MIT License - see the LICENSE file for details.

## 🆘 Support

For support and questions:
- Create an issue in the GitHub repository
- Check the documentation and code comments
- Review the sample data and usage examples

## 🎯 Roadmap

### Phase 1: Core Functionality ✅
- Basic template and document management
- JSON data persistence
- Console interface
- Service architecture

### Phase 2: WPF Implementation (Planned)
- Full WPF user interface
- MVVM implementation
- Rich data binding
- Modern UI styling

### Phase 3: Advanced Features (Planned)
- Database persistence
- Advanced search and filtering
- Document versioning
- User authentication and authorization
- Email notifications
- API endpoints

### Phase 4: Enterprise Features (Planned)
- Multi-tenant support
- Workflow automation
- Integration APIs
- Advanced reporting
- Audit trails
- Performance optimization

---

**Built with ❤️ using .NET 8 and modern software architecture principles.**