using DocumentManagement.Models;

namespace DocumentManagementSystem.Converters;

/// <summary>
/// Value converter for converting status enums to colors, text, and visibility
/// Would be used in WPF as: {Binding Status, Converter={StaticResource StatusConverter}}
/// </summary>
public static class StatusConverter
{
    public static string ToColorHex(TemplateStatus status)
    {
        return status switch
        {
            TemplateStatus.Uploaded => "#FFA500",      // Orange
            TemplateStatus.Converting => "#FFD700",    // Gold
            TemplateStatus.Converted => "#32CD32",     // LimeGreen
            TemplateStatus.ConversionFailed => "#FF4500", // Red
            TemplateStatus.Active => "#228B22",        // ForestGreen
            TemplateStatus.Inactive => "#808080",      // Gray
            _ => "#000000"                             // Black
        };
    }

    public static string ToColorHex(DocumentStatus status)
    {
        return status switch
        {
            DocumentStatus.Pending => "#FFA500",       // Orange
            DocumentStatus.InProgress => "#4169E1",    // RoyalBlue
            DocumentStatus.Completed => "#32CD32",     // LimeGreen
            DocumentStatus.Rejected => "#DC143C",      // Crimson
            DocumentStatus.Archived => "#708090",      // SlateGray
            _ => "#000000"                             // Black
        };
    }

    public static string ToColorHex(DocumentPriority priority)
    {
        return priority switch
        {
            DocumentPriority.Low => "#90EE90",         // LightGreen
            DocumentPriority.Normal => "#87CEEB",      // SkyBlue
            DocumentPriority.High => "#FFA500",        // Orange
            DocumentPriority.Urgent => "#FF4500",      // Red
            _ => "#000000"                             // Black
        };
    }

    public static string ToDisplayText(TemplateStatus status)
    {
        return status switch
        {
            TemplateStatus.ConversionFailed => "Conversion Failed",
            _ => status.ToString()
        };
    }

    public static string ToDisplayText(DocumentStatus status)
    {
        return status switch
        {
            DocumentStatus.InProgress => "In Progress",
            _ => status.ToString()
        };
    }

    public static string ToDisplayText(DocumentPriority priority)
    {
        return priority.ToString();
    }

    public static bool ToVisibility(TemplateStatus status, TemplateStatus targetStatus)
    {
        return status == targetStatus;
    }

    public static bool ToVisibility(DocumentStatus status, DocumentStatus targetStatus)
    {
        return status == targetStatus;
    }

    public static string GetStatusIcon(TemplateStatus status)
    {
        return status switch
        {
            TemplateStatus.Uploaded => "📤",
            TemplateStatus.Converting => "🔄",
            TemplateStatus.Converted => "✅",
            TemplateStatus.ConversionFailed => "❌",
            TemplateStatus.Active => "🟢",
            TemplateStatus.Inactive => "⚪",
            _ => "❓"
        };
    }

    public static string GetStatusIcon(DocumentStatus status)
    {
        return status switch
        {
            DocumentStatus.Pending => "⏳",
            DocumentStatus.InProgress => "🔄",
            DocumentStatus.Completed => "✅",
            DocumentStatus.Rejected => "❌",
            DocumentStatus.Archived => "📦",
            _ => "❓"
        };
    }

    public static string GetPriorityIcon(DocumentPriority priority)
    {
        return priority switch
        {
            DocumentPriority.Low => "🔵",
            DocumentPriority.Normal => "🟡",
            DocumentPriority.High => "🟠",
            DocumentPriority.Urgent => "🔴",
            _ => "⚪"
        };
    }
}