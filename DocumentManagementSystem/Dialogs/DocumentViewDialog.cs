using DocumentManagement.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace DocumentManagementSystem.Dialogs;

/// <summary>
/// Dialog for viewing document details with processing timeline
/// In WPF, this would show rich document information and history
/// </summary>
public class DocumentViewDialog
{
    public async Task ShowDocumentAsync(IncomingDocument document)
    {
        Console.WriteLine("Document Details");
        Console.WriteLine("================");
        Console.WriteLine($"ID: {document.Id}");
        Console.WriteLine($"Title: {document.Title}");
        Console.WriteLine($"Document Number: {document.DocumentNumber}");
        Console.WriteLine($"Sender: {document.Sender}");
        Console.WriteLine($"Organization: {document.Organization}");
        Console.WriteLine($"Status: {document.Status}");
        Console.WriteLine($"Priority: {document.Priority}");
        Console.WriteLine($"Received: {document.ReceivedDate:yyyy-MM-dd HH:mm:ss}");
        Console.WriteLine($"Modified: {document.ModifiedDate:yyyy-MM-dd HH:mm:ss}");
        
        if (document.CompletedDate.HasValue)
        {
            Console.WriteLine($"Completed: {document.CompletedDate:yyyy-MM-dd HH:mm:ss}");
        }

        Console.WriteLine($"Template ID: {document.TemplateId}");
        Console.WriteLine();
        Console.WriteLine("Notes:");
        Console.WriteLine(string.IsNullOrEmpty(document.Notes) ? "(No notes)" : document.Notes);
        Console.WriteLine();

        if (document.ProcessingHistory.Any())
        {
            Console.WriteLine("Processing History:");
            Console.WriteLine("-------------------");
            foreach (var entry in document.ProcessingHistory.OrderBy(h => h.Timestamp))
            {
                Console.WriteLine($"{entry.Timestamp:yyyy-MM-dd HH:mm:ss} - {entry.Action} (by {entry.User})");
                if (!string.IsNullOrEmpty(entry.Notes))
                {
                    Console.WriteLine($"  Notes: {entry.Notes}");
                }
            }
        }
        else
        {
            Console.WriteLine("No processing history available.");
        }

        Console.WriteLine();
        Console.WriteLine("In WPF version, this would display:");
        Console.WriteLine("- Rich document preview");
        Console.WriteLine("- Interactive timeline");
        Console.WriteLine("- Attachment management");
        Console.WriteLine("- Status change controls");
        Console.WriteLine("- Print and export options");
    }
}

/* Example XAML for WPF version:
<Window x:Class="DocumentManagementSystem.Dialogs.DocumentViewDialog"
        xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
        xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
        Title="Document Details" Height="700" Width="800"
        WindowStartupLocation="CenterParent">
    <Grid Margin="20">
        <Grid.RowDefinitions>
            <RowDefinition Height="Auto"/>
            <RowDefinition Height="*"/>
            <RowDefinition Height="Auto"/>
        </Grid.RowDefinitions>
        
        <!-- Document Header -->
        <Border Grid.Row="0" Background="LightBlue" Padding="15" CornerRadius="5">
            <Grid>
                <Grid.ColumnDefinitions>
                    <ColumnDefinition Width="*"/>
                    <ColumnDefinition Width="Auto"/>
                </Grid.ColumnDefinitions>
                
                <StackPanel Grid.Column="0">
                    <TextBlock Text="{Binding Document.Title}" FontSize="18" FontWeight="Bold"/>
                    <TextBlock Text="{Binding Document.DocumentNumber}" FontSize="14" Foreground="DarkBlue"/>
                    <StackPanel Orientation="Horizontal" Margin="0,5,0,0">
                        <TextBlock Text="Status:" FontWeight="Bold" Margin="0,0,5,0"/>
                        <TextBlock Text="{Binding Document.Status}" 
                                  Foreground="{Binding Document.Status, Converter={StaticResource StatusToColorConverter}}"/>
                        <TextBlock Text="Priority:" FontWeight="Bold" Margin="20,0,5,0"/>
                        <TextBlock Text="{Binding Document.Priority}" 
                                  Foreground="{Binding Document.Priority, Converter={StaticResource PriorityToColorConverter}}"/>
                    </StackPanel>
                </StackPanel>
                
                <StackPanel Grid.Column="1" HorizontalAlignment="Right">
                    <Button Content="Edit" Width="80" Height="30" Margin="0,0,0,5" 
                            Command="{Binding EditDocumentCommand}"/>
                    <Button Content="Process" Width="80" Height="30" 
                            Command="{Binding ProcessDocumentCommand}"/>
                </StackPanel>
            </Grid>
        </Border>
        
        <!-- Tabbed Content -->
        <TabControl Grid.Row="1" Margin="0,10,0,10">
            <TabItem Header="Details">
                <ScrollViewer VerticalScrollBarVisibility="Auto">
                    <Grid Margin="10">
                        <Grid.RowDefinitions>
                            <RowDefinition Height="Auto"/>
                            <RowDefinition Height="Auto"/>
                            <RowDefinition Height="Auto"/>
                            <RowDefinition Height="Auto"/>
                            <RowDefinition Height="Auto"/>
                            <RowDefinition Height="Auto"/>
                            <RowDefinition Height="*"/>
                        </Grid.RowDefinitions>
                        <Grid.ColumnDefinitions>
                            <ColumnDefinition Width="120"/>
                            <ColumnDefinition Width="*"/>
                        </Grid.ColumnDefinitions>
                        
                        <TextBlock Grid.Row="0" Grid.Column="0" Text="Sender:" FontWeight="Bold"/>
                        <TextBlock Grid.Row="0" Grid.Column="1" Text="{Binding Document.Sender}"/>
                        
                        <TextBlock Grid.Row="1" Grid.Column="0" Text="Organization:" FontWeight="Bold"/>
                        <TextBlock Grid.Row="1" Grid.Column="1" Text="{Binding Document.Organization}"/>
                        
                        <TextBlock Grid.Row="2" Grid.Column="0" Text="Received:" FontWeight="Bold"/>
                        <TextBlock Grid.Row="2" Grid.Column="1" Text="{Binding Document.ReceivedDate, StringFormat=yyyy-MM-dd HH:mm:ss}"/>
                        
                        <TextBlock Grid.Row="3" Grid.Column="0" Text="Modified:" FontWeight="Bold"/>
                        <TextBlock Grid.Row="3" Grid.Column="1" Text="{Binding Document.ModifiedDate, StringFormat=yyyy-MM-dd HH:mm:ss}"/>
                        
                        <TextBlock Grid.Row="4" Grid.Column="0" Text="Completed:" FontWeight="Bold"
                                  Visibility="{Binding Document.CompletedDate, Converter={StaticResource NullToVisibilityConverter}}"/>
                        <TextBlock Grid.Row="4" Grid.Column="1" Text="{Binding Document.CompletedDate, StringFormat=yyyy-MM-dd HH:mm:ss}"
                                  Visibility="{Binding Document.CompletedDate, Converter={StaticResource NullToVisibilityConverter}}"/>
                        
                        <TextBlock Grid.Row="5" Grid.Column="0" Text="Template:" FontWeight="Bold"/>
                        <TextBlock Grid.Row="5" Grid.Column="1" Text="{Binding AssociatedTemplateName}"/>
                        
                        <TextBlock Grid.Row="6" Grid.Column="0" Text="Notes:" FontWeight="Bold" VerticalAlignment="Top"/>
                        <TextBox Grid.Row="6" Grid.Column="1" Text="{Binding Document.Notes}" 
                                TextWrapping="Wrap" IsReadOnly="True" BorderThickness="0" Background="Transparent"/>
                    </Grid>
                </ScrollViewer>
            </TabItem>
            
            <TabItem Header="Processing History">
                <ListView ItemsSource="{Binding Document.ProcessingHistory}" Margin="10">
                    <ListView.View>
                        <GridView>
                            <GridViewColumn Header="Date/Time" Width="150" 
                                          DisplayMemberBinding="{Binding Timestamp, StringFormat=yyyy-MM-dd HH:mm:ss}"/>
                            <GridViewColumn Header="Action" Width="200" DisplayMemberBinding="{Binding Action}"/>
                            <GridViewColumn Header="User" Width="100" DisplayMemberBinding="{Binding User}"/>
                            <GridViewColumn Header="Notes" Width="250" DisplayMemberBinding="{Binding Notes}"/>
                        </GridView>
                    </ListView.View>
                </ListView>
            </TabItem>
            
            <TabItem Header="Attachments">
                <Grid Margin="10">
                    <Grid.RowDefinitions>
                        <RowDefinition Height="Auto"/>
                        <RowDefinition Height="*"/>
                    </Grid.RowDefinitions>
                    
                    <ToolBar Grid.Row="0">
                        <Button Content="Add Attachment" Command="{Binding AddAttachmentCommand}"/>
                        <Button Content="Remove Selected" Command="{Binding RemoveAttachmentCommand}"/>
                        <Button Content="Download" Command="{Binding DownloadAttachmentCommand}"/>
                    </ToolBar>
                    
                    <ListView Grid.Row="1" ItemsSource="{Binding Attachments}" Margin="0,5,0,0">
                        <ListView.View>
                            <GridView>
                                <GridViewColumn Header="Name" Width="200" DisplayMemberBinding="{Binding Name}"/>
                                <GridViewColumn Header="Size" Width="100" 
                                              DisplayMemberBinding="{Binding Size, Converter={StaticResource FileSizeConverter}}"/>
                                <GridViewColumn Header="Type" Width="100" DisplayMemberBinding="{Binding Type}"/>
                                <GridViewColumn Header="Added" Width="150" 
                                              DisplayMemberBinding="{Binding AddedDate, StringFormat=yyyy-MM-dd HH:mm}"/>
                            </GridView>
                        </ListView.View>
                    </ListView>
                </Grid>
            </TabItem>
        </TabControl>
        
        <!-- Action Buttons -->
        <StackPanel Grid.Row="2" Orientation="Horizontal" HorizontalAlignment="Right">
            <Button Content="Print" Width="80" Height="30" Margin="0,0,10,0" 
                    Command="{Binding PrintCommand}"/>
            <Button Content="Export" Width="80" Height="30" Margin="0,0,10,0" 
                    Command="{Binding ExportCommand}"/>
            <Button Content="Close" Width="80" Height="30" 
                    Command="{Binding CloseCommand}"/>
        </StackPanel>
    </Grid>
</Window>
*/