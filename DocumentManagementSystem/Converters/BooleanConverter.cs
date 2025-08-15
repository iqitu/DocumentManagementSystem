namespace DocumentManagementSystem.Converters;

/// <summary>
/// Value converter for boolean values to various representations
/// Would be used in WPF as: {Binding IsActive, Converter={StaticResource BooleanConverter}}
/// </summary>
public static class BooleanConverter
{
    public static string ToVisibility(bool value)
    {
        return value ? "Visible" : "Collapsed";
    }

    public static string ToInverseVisibility(bool value)
    {
        return value ? "Collapsed" : "Visible";
    }

    public static string ToYesNo(bool value)
    {
        return value ? "Yes" : "No";
    }

    public static string ToEnabledDisabled(bool value)
    {
        return value ? "Enabled" : "Disabled";
    }

    public static string ToActiveInactive(bool value)
    {
        return value ? "Active" : "Inactive";
    }

    public static string ToOnOff(bool value)
    {
        return value ? "On" : "Off";
    }

    public static string ToIcon(bool value)
    {
        return value ? "✅" : "❌";
    }

    public static string ToColorHex(bool value)
    {
        return value ? "#32CD32" : "#FF4500"; // Green for true, Red for false
    }

    public static bool FromString(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return false;

        var normalized = value.Trim().ToLowerInvariant();
        return normalized switch
        {
            "true" or "yes" or "1" or "on" or "enabled" or "active" => true,
            "false" or "no" or "0" or "off" or "disabled" or "inactive" => false,
            _ => bool.TryParse(value, out var result) && result
        };
    }

    public static double ToOpacity(bool value)
    {
        return value ? 1.0 : 0.5;
    }

    public static int ToIntValue(bool value)
    {
        return value ? 1 : 0;
    }
}