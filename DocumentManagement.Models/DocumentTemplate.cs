using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace DocumentManagement.Models;

public class DocumentTemplate : INotifyPropertyChanged
{
    private string _name = string.Empty;
    private string _category = string.Empty;
    private string _description = string.Empty;
    private string _originalFilePath = string.Empty;
    private string _xamlFilePath = string.Empty;
    private TemplateStatus _status = TemplateStatus.Uploaded;
    private FileType _fileType = FileType.Unknown;
    private long _fileSize;
    private DateTime _createdDate = DateTime.Now;
    private DateTime _modifiedDate = DateTime.Now;
    private DateTime? _convertedDate;

    public string Id { get; set; } = Guid.NewGuid().ToString();

    public string Name
    {
        get => _name;
        set => SetProperty(ref _name, value);
    }

    public string Category
    {
        get => _category;
        set => SetProperty(ref _category, value);
    }

    public string Description
    {
        get => _description;
        set => SetProperty(ref _description, value);
    }

    public string OriginalFilePath
    {
        get => _originalFilePath;
        set => SetProperty(ref _originalFilePath, value);
    }

    public string XamlFilePath
    {
        get => _xamlFilePath;
        set => SetProperty(ref _xamlFilePath, value);
    }

    public TemplateStatus Status
    {
        get => _status;
        set => SetProperty(ref _status, value);
    }

    public FileType FileType
    {
        get => _fileType;
        set => SetProperty(ref _fileType, value);
    }

    public long FileSize
    {
        get => _fileSize;
        set => SetProperty(ref _fileSize, value);
    }

    public DateTime CreatedDate
    {
        get => _createdDate;
        set => SetProperty(ref _createdDate, value);
    }

    public DateTime ModifiedDate
    {
        get => _modifiedDate;
        set => SetProperty(ref _modifiedDate, value);
    }

    public DateTime? ConvertedDate
    {
        get => _convertedDate;
        set => SetProperty(ref _convertedDate, value);
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
}