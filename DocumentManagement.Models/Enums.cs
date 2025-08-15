namespace DocumentManagement.Models;

public enum TemplateStatus
{
    Uploaded,
    Converting,
    Converted,
    ConversionFailed,
    Active,
    Inactive
}

public enum DocumentStatus
{
    Pending,
    InProgress,
    Completed,
    Rejected,
    Archived
}

public enum DocumentPriority
{
    Low,
    Normal,
    High,
    Urgent
}

public enum FileType
{
    Unknown,
    Word,
    Excel,
    PDF,
    Text,
    XAML
}
