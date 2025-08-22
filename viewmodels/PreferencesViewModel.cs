using System;
using System.ComponentModel;
using TheWordForge.services;

namespace TheWordForge;

public class PreferencesViewModel : INotifyPropertyChanged
{
    public Array TransitionModes => Enum.GetValues(typeof(TransitionMode));
    public Array TransitionSpeeds => Enum.GetValues(typeof(TransitionSpeed));

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

    public TransitionSpeed TransitionSpeed
    {
        get => PreferencesService.Speed;
        set
        {
            if (PreferencesService.Speed != value)
            {
                PreferencesService.Speed = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(TransitionSpeed)));
            }
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;
}
