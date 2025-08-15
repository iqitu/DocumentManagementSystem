using System.Globalization;

namespace DocumentManagementSystem.Converters;

/// <summary>
/// Value converter for formatting file sizes in a human-readable format
/// Would be used as [ValueConverter] in WPF binding: {Binding FileSize, Converter={StaticResource FileSizeConverter}}
/// </summary>
public class FileSizeConverter
{
    public static string Convert(long bytes)
    {
        if (bytes == 0) return "0 B";
        
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

    public static object ConvertBack(string value)
    {
        // Parse formatted file size back to bytes
        if (string.IsNullOrWhiteSpace(value))
            return 0L;

        var parts = value.Split(' ');
        if (parts.Length != 2)
            return 0L;

        if (!double.TryParse(parts[0], out var size))
            return 0L;

        var unit = parts[1].ToUpperInvariant();
        var multiplier = unit switch
        {
            "B" => 1L,
            "KB" => 1024L,
            "MB" => 1024L * 1024L,
            "GB" => 1024L * 1024L * 1024L,
            "TB" => 1024L * 1024L * 1024L * 1024L,
            _ => 1L
        };

        return (long)(size * multiplier);
    }
}