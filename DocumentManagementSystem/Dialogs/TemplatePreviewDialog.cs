using DocumentManagement.Models;
using System;
using System.IO;
using System.Threading.Tasks;

namespace DocumentManagementSystem.Dialogs;

/// <summary>
/// Dialog for previewing template XAML content
/// In WPF, this would display the actual XAML rendering
/// </summary>
public class TemplatePreviewDialog
{
    public async Task ShowPreviewAsync(DocumentTemplate template)
    {
        Console.WriteLine("Template Preview");
        Console.WriteLine("================");
        Console.WriteLine($"Template: {template.Name}");
        Console.WriteLine($"Status: {template.Status}");
        Console.WriteLine($"File Type: {template.FileType}");
        Console.WriteLine($"File Size: {template.FileSize} bytes");
        Console.WriteLine();

        if (!string.IsNullOrEmpty(template.XamlFilePath) && File.Exists(template.XamlFilePath))
        {
            Console.WriteLine("XAML Content Preview:");
            Console.WriteLine("--------------------");
            var xamlContent = await File.ReadAllTextAsync(template.XamlFilePath);
            
            // Show first 500 characters of XAML
            var preview = xamlContent.Length > 500 ? xamlContent[..500] + "..." : xamlContent;
            Console.WriteLine(preview);
        }
        else
        {
            Console.WriteLine("XAML preview not available - template not yet converted.");
        }

        Console.WriteLine();
        Console.WriteLine("In WPF version, this would display:");
        Console.WriteLine("- Live XAML rendering in preview pane");
        Console.WriteLine("- Syntax-highlighted XAML editor");
        Console.WriteLine("- Zoom and pan controls");
        Console.WriteLine("- Export options");
    }
}

/* Example XAML for WPF version:
<Window x:Class="DocumentManagementSystem.Dialogs.TemplatePreviewDialog"
        xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
        xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
        Title="Template Preview" Height="600" Width="900"
        WindowStartupLocation="CenterParent">
    <Grid>
        <Grid.RowDefinitions>
            <RowDefinition Height="Auto"/>
            <RowDefinition Height="*"/>
            <RowDefinition Height="Auto"/>
        </Grid.RowDefinitions>
        
        <!-- Template Info Header -->
        <Border Grid.Row="0" Background="LightGray" Padding="10">
            <StackPanel Orientation="Horizontal">
                <TextBlock Text="{Binding Template.Name}" FontWeight="Bold" FontSize="16" Margin="0,0,20,0"/>
                <TextBlock Text="{Binding Template.Status, Converter={StaticResource StatusConverter}}" 
                          Foreground="{Binding Template.Status, Converter={StaticResource StatusToColorConverter}}"
                          Margin="0,0,20,0"/>
                <TextBlock Text="{Binding Template.FileType}" Margin="0,0,20,0"/>
                <TextBlock Text="{Binding Template.FileSize, Converter={StaticResource FileSizeConverter}}"/>
            </StackPanel>
        </Border>
        
        <!-- Tabbed Preview -->
        <TabControl Grid.Row="1" Margin="10">
            <TabItem Header="Visual Preview">
                <ScrollViewer ZoomMode="Enabled" HorizontalScrollBarVisibility="Auto" VerticalScrollBarVisibility="Auto">
                    <ContentPresenter x:Name="XamlPreview" Content="{Binding RenderedTemplate}"/>
                </ScrollViewer>
            </TabItem>
            <TabItem Header="XAML Source">
                <Grid>
                    <Grid.RowDefinitions>
                        <RowDefinition Height="Auto"/>
                        <RowDefinition Height="*"/>
                    </Grid.RowDefinitions>
                    <ToolBar Grid.Row="0">
                        <Button Content="Format" Command="{Binding FormatXamlCommand}"/>
                        <Button Content="Validate" Command="{Binding ValidateXamlCommand}"/>
                        <Button Content="Save Changes" Command="{Binding SaveXamlCommand}"/>
                    </ToolBar>
                    <TextBox Grid.Row="1" Text="{Binding XamlContent, UpdateSourceTrigger=PropertyChanged}"
                             FontFamily="Consolas" FontSize="12" AcceptsReturn="True" 
                             HorizontalScrollBarVisibility="Auto" VerticalScrollBarVisibility="Auto"/>
                </Grid>
            </TabItem>
        </TabControl>
        
        <!-- Action Buttons -->
        <StackPanel Grid.Row="2" Orientation="Horizontal" HorizontalAlignment="Right" Margin="10">
            <Button Content="Edit Template" Width="100" Height="30" Margin="0,0,10,0" 
                    Command="{Binding EditTemplateCommand}"/>
            <Button Content="Export" Width="80" Height="30" Margin="0,0,10,0" 
                    Command="{Binding ExportCommand}"/>
            <Button Content="Close" Width="80" Height="30" 
                    Command="{Binding CloseCommand}"/>
        </StackPanel>
    </Grid>
</Window>
*/