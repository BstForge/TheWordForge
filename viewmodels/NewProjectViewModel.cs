using System.Collections.Generic;
using System.ComponentModel;

namespace TheWordForge;

public class NewProjectViewModel : INotifyPropertyChanged
{
    private string _title = string.Empty;
    private string _authorName = string.Empty;
    private string _genre = "Other";
    private string _saveLocation = string.Empty;

    public event PropertyChangedEventHandler? PropertyChanged;

    public List<string> Genres { get; } = new() { "Fantasy", "Sci-Fi", "Romance", "Mystery", "Horror", "Other" };

    public string Title
    {
        get => _title;
        set
        {
            if (_title != value)
            {
                _title = value;
                OnPropertyChanged(nameof(Title));
            }
        }
    }

    public string AuthorName
    {
        get => _authorName;
        set
        {
            if (_authorName != value)
            {
                _authorName = value;
                OnPropertyChanged(nameof(AuthorName));
            }
        }
    }

    public string Genre
    {
        get => _genre;
        set
        {
            if (_genre != value)
            {
                _genre = value;
                OnPropertyChanged(nameof(Genre));
            }
        }
    }

    public string SaveLocation
    {
        get => _saveLocation;
        set
        {
            if (_saveLocation != value)
            {
                _saveLocation = value;
                OnPropertyChanged(nameof(SaveLocation));
            }
        }
    }

    private void OnPropertyChanged(string name) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
}
