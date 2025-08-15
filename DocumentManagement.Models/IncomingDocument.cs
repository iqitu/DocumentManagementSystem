using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace DocumentManagement.Models;

public class IncomingDocument : INotifyPropertyChanged
{
    private string _title = string.Empty;
    private string _documentNumber = string.Empty;
    private string _sender = string.Empty;
    private string _organization = string.Empty;
    private DocumentStatus _status = DocumentStatus.Pending;
    private DocumentPriority _priority = DocumentPriority.Normal;
    private string _notes = string.Empty;
    private string _templateId = string.Empty;
    private DateTime _receivedDate = DateTime.Now;
    private DateTime _modifiedDate = DateTime.Now;
    private DateTime? _completedDate;
    private List<ProcessingHistoryEntry> _processingHistory = new();

    public string Id { get; set; } = Guid.NewGuid().ToString();

    public string Title
    {
        get => _title;
        set => SetProperty(ref _title, value);
    }

    public string DocumentNumber
    {
        get => _documentNumber;
        set => SetProperty(ref _documentNumber, value);
    }

    public string Sender
    {
        get => _sender;
        set => SetProperty(ref _sender, value);
    }

    public string Organization
    {
        get => _organization;
        set => SetProperty(ref _organization, value);
    }

    public DocumentStatus Status
    {
        get => _status;
        set
        {
            if (SetProperty(ref _status, value))
            {
                AddProcessingHistoryEntry($"Status changed to {value}");
                if (value == DocumentStatus.Completed || value == DocumentStatus.Rejected)
                {
                    CompletedDate = DateTime.Now;
                }
            }
        }
    }

    public DocumentPriority Priority
    {
        get => _priority;
        set => SetProperty(ref _priority, value);
    }

    public string Notes
    {
        get => _notes;
        set => SetProperty(ref _notes, value);
    }

    public string TemplateId
    {
        get => _templateId;
        set => SetProperty(ref _templateId, value);
    }

    public DateTime ReceivedDate
    {
        get => _receivedDate;
        set => SetProperty(ref _receivedDate, value);
    }

    public DateTime ModifiedDate
    {
        get => _modifiedDate;
        set => SetProperty(ref _modifiedDate, value);
    }

    public DateTime? CompletedDate
    {
        get => _completedDate;
        set => SetProperty(ref _completedDate, value);
    }

    public List<ProcessingHistoryEntry> ProcessingHistory
    {
        get => _processingHistory;
        set => SetProperty(ref _processingHistory, value);
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    protected bool SetProperty<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
    {
        if (EqualityComparer<T>.Default.Equals(field, value))
            return false;

        field = value;
        ModifiedDate = DateTime.Now;
        OnPropertyChanged(propertyName);
        return true;
    }

    private void AddProcessingHistoryEntry(string action)
    {
        ProcessingHistory.Add(new ProcessingHistoryEntry
        {
            Timestamp = DateTime.Now,
            Action = action,
            User = Environment.UserName
        });
    }
}

public class ProcessingHistoryEntry
{
    public DateTime Timestamp { get; set; }
    public string Action { get; set; } = string.Empty;
    public string User { get; set; } = string.Empty;
    public string Notes { get; set; } = string.Empty;
}