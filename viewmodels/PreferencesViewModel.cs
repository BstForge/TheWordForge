using System;
using System.ComponentModel;
using TheWordForge.services;

namespace TheWordForge;

public class PreferencesViewModel : INotifyPropertyChanged
{
    public Array TransitionModes => Enum.GetValues(typeof(TransitionMode));

    public TransitionMode TransitionMode
    {
        get => PreferencesService.Transition;
        set
        {
            if (PreferencesService.Transition != value)
            {
                PreferencesService.Transition = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(TransitionMode)));
            }
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;
}
