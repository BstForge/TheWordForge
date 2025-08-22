using System.ComponentModel;
using TheWordForge.services;

namespace TheWordForge;

public class PreferencesViewModel : INotifyPropertyChanged
{
    public bool TransitionsEnabled
    {
        get => PreferencesService.TransitionsEnabled;
        set
        {
            if (PreferencesService.TransitionsEnabled != value)
            {
                PreferencesService.TransitionsEnabled = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(TransitionsEnabled)));
            }
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;
}
