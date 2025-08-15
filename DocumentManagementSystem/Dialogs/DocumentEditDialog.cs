using DocumentManagement.Models;
using System;
using System.Threading.Tasks;

namespace DocumentManagementSystem.Dialogs;

/// <summary>
/// Dialog for editing document details
/// In WPF, this would be a Window with XAML UI and data binding
/// </summary>
public class DocumentEditDialog
{
    public async Task<IncomingDocument?> ShowDialogAsync(IncomingDocument? document = null)
    {
        var isEditing = document != null;
        var workingDocument = document ?? new IncomingDocument();

        Console.WriteLine(isEditing ? "Edit Document" : "Create New Document");
        Console.WriteLine("==================");

        Console.Write($"Title [{workingDocument.Title}]: ");
        var title = Console.ReadLine();
        if (!string.IsNullOrWhiteSpace(title))
            workingDocument.Title = title;

        Console.Write($"Document Number [{workingDocument.DocumentNumber}]: ");
        var docNumber = Console.ReadLine();
        if (!string.IsNullOrWhiteSpace(docNumber))
            workingDocument.DocumentNumber = docNumber;

        Console.Write($"Sender [{workingDocument.Sender}]: ");
        var sender = Console.ReadLine();
        if (!string.IsNullOrWhiteSpace(sender))
            workingDocument.Sender = sender;

        Console.Write($"Organization [{workingDocument.Organization}]: ");
        var organization = Console.ReadLine();
        if (!string.IsNullOrWhiteSpace(organization))
            workingDocument.Organization = organization;

        // Status selection
        Console.WriteLine("\nStatus Options:");
        var statusValues = Enum.GetValues<DocumentStatus>();
        for (int i = 0; i < statusValues.Length; i++)
        {
            Console.WriteLine($"{i + 1}. {statusValues[i]}");
        }
        Console.Write($"Select Status [{(int)workingDocument.Status + 1}]: ");
        if (int.TryParse(Console.ReadLine(), out var statusChoice) && 
            statusChoice >= 1 && statusChoice <= statusValues.Length)
        {
            workingDocument.Status = statusValues[statusChoice - 1];
        }

        // Priority selection
        Console.WriteLine("\nPriority Options:");
        var priorityValues = Enum.GetValues<DocumentPriority>();
        for (int i = 0; i < priorityValues.Length; i++)
        {
            Console.WriteLine($"{i + 1}. {priorityValues[i]}");
        }
        Console.Write($"Select Priority [{(int)workingDocument.Priority + 1}]: ");
        if (int.TryParse(Console.ReadLine(), out var priorityChoice) && 
            priorityChoice >= 1 && priorityChoice <= priorityValues.Length)
        {
            workingDocument.Priority = priorityValues[priorityChoice - 1];
        }

        Console.Write($"Notes [{workingDocument.Notes}]: ");
        var notes = Console.ReadLine();
        if (!string.IsNullOrWhiteSpace(notes))
            workingDocument.Notes = notes;

        Console.WriteLine("\nDocument Details:");
        Console.WriteLine($"Title: {workingDocument.Title}");
        Console.WriteLine($"Document Number: {workingDocument.DocumentNumber}");
        Console.WriteLine($"Sender: {workingDocument.Sender}");
        Console.WriteLine($"Organization: {workingDocument.Organization}");
        Console.WriteLine($"Status: {workingDocument.Status}");
        Console.WriteLine($"Priority: {workingDocument.Priority}");
        Console.WriteLine($"Notes: {workingDocument.Notes}");

        Console.Write("\nSave changes? (y/n): ");
        var save = Console.ReadLine()?.ToLowerInvariant() == "y";

        return save ? workingDocument : null;
    }
}

/* Example XAML for WPF version:
<Window x:Class="DocumentManagementSystem.Dialogs.DocumentEditDialog"
        xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
        xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
        xmlns:models="clr-namespace:DocumentManagement.Models;assembly=DocumentManagement.Models"
        Title="Document Details" Height="500" Width="600"
        WindowStartupLocation="CenterParent">
    <Grid Margin="20">
        <Grid.RowDefinitions>
            <RowDefinition Height="Auto"/>
            <RowDefinition Height="Auto"/>
            <RowDefinition Height="Auto"/>
            <RowDefinition Height="Auto"/>
            <RowDefinition Height="Auto"/>
            <RowDefinition Height="Auto"/>
            <RowDefinition Height="Auto"/>
            <RowDefinition Height="Auto"/>
            <RowDefinition Height="*"/>
            <RowDefinition Height="Auto"/>
        </Grid.RowDefinitions>
        
        <TextBlock Grid.Row="0" Text="{Binding IsEditing, Converter={StaticResource EditModeConverter}}" 
                   FontSize="18" FontWeight="Bold" Margin="0,0,0,20"/>
        
        <Grid Grid.Row="1" Margin="0,5">
            <Grid.ColumnDefinitions>
                <ColumnDefinition Width="120"/>
                <ColumnDefinition Width="*"/>
            </Grid.ColumnDefinitions>
            <TextBlock Grid.Column="0" Text="Title:" VerticalAlignment="Center"/>
            <TextBox Grid.Column="1" Text="{Binding Document.Title, UpdateSourceTrigger=PropertyChanged}"/>
        </Grid>
        
        <Grid Grid.Row="2" Margin="0,5">
            <Grid.ColumnDefinitions>
                <ColumnDefinition Width="120"/>
                <ColumnDefinition Width="*"/>
            </Grid.ColumnDefinitions>
            <TextBlock Grid.Column="0" Text="Document Number:" VerticalAlignment="Center"/>
            <TextBox Grid.Column="1" Text="{Binding Document.DocumentNumber, UpdateSourceTrigger=PropertyChanged}"/>
        </Grid>
        
        <Grid Grid.Row="3" Margin="0,5">
            <Grid.ColumnDefinitions>
                <ColumnDefinition Width="120"/>
                <ColumnDefinition Width="*"/>
            </Grid.ColumnDefinitions>
            <TextBlock Grid.Column="0" Text="Sender:" VerticalAlignment="Center"/>
            <TextBox Grid.Column="1" Text="{Binding Document.Sender, UpdateSourceTrigger=PropertyChanged}"/>
        </Grid>
        
        <Grid Grid.Row="4" Margin="0,5">
            <Grid.ColumnDefinitions>
                <ColumnDefinition Width="120"/>
                <ColumnDefinition Width="*"/>
            </Grid.ColumnDefinitions>
            <TextBlock Grid.Column="0" Text="Organization:" VerticalAlignment="Center"/>
            <TextBox Grid.Column="1" Text="{Binding Document.Organization, UpdateSourceTrigger=PropertyChanged}"/>
        </Grid>
        
        <Grid Grid.Row="5" Margin="0,5">
            <Grid.ColumnDefinitions>
                <ColumnDefinition Width="120"/>
                <ColumnDefinition Width="*"/>
            </Grid.ColumnDefinitions>
            <TextBlock Grid.Column="0" Text="Status:" VerticalAlignment="Center"/>
            <ComboBox Grid.Column="1" ItemsSource="{Binding StatusOptions}" 
                      SelectedItem="{Binding Document.Status}"/>
        </Grid>
        
        <Grid Grid.Row="6" Margin="0,5">
            <Grid.ColumnDefinitions>
                <ColumnDefinition Width="120"/>
                <ColumnDefinition Width="*"/>
            </Grid.ColumnDefinitions>
            <TextBlock Grid.Column="0" Text="Priority:" VerticalAlignment="Center"/>
            <ComboBox Grid.Column="1" ItemsSource="{Binding PriorityOptions}" 
                      SelectedItem="{Binding Document.Priority}"/>
        </Grid>
        
        <Grid Grid.Row="7" Margin="0,5">
            <Grid.ColumnDefinitions>
                <ColumnDefinition Width="120"/>
                <ColumnDefinition Width="*"/>
            </Grid.ColumnDefinitions>
            <TextBlock Grid.Column="0" Text="Template:" VerticalAlignment="Center"/>
            <ComboBox Grid.Column="1" ItemsSource="{Binding AvailableTemplates}" 
                      SelectedValue="{Binding Document.TemplateId}" SelectedValuePath="Id"
                      DisplayMemberPath="Name"/>
        </Grid>
        
        <Grid Grid.Row="8" Margin="0,5">
            <Grid.ColumnDefinitions>
                <ColumnDefinition Width="120"/>
                <ColumnDefinition Width="*"/>
            </Grid.ColumnDefinitions>
            <TextBlock Grid.Column="0" Text="Notes:" VerticalAlignment="Top" Margin="0,5,0,0"/>
            <TextBox Grid.Column="1" Text="{Binding Document.Notes, UpdateSourceTrigger=PropertyChanged}" 
                     TextWrapping="Wrap" AcceptsReturn="True" Height="80"/>
        </Grid>
        
        <StackPanel Grid.Row="9" Orientation="Horizontal" HorizontalAlignment="Right" Margin="0,20,0,0">
            <Button Content="Save" Width="80" Height="30" Margin="0,0,10,0" 
                    Command="{Binding SaveCommand}"/>
            <Button Content="Cancel" Width="80" Height="30" 
                    Command="{Binding CancelCommand}"/>
        </StackPanel>
    </Grid>
</Window>
*/