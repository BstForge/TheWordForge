using System;
using Avalonia.Animation;

namespace TheWordForge.services;

public static class PreferencesService
{
    private static bool _transitionsEnabled;

    public static bool TransitionsEnabled
    {
        get => _transitionsEnabled;
        set
        {
            if (_transitionsEnabled != value)
            {
                _transitionsEnabled = value;
                TransitionChanged?.Invoke(null, EventArgs.Empty);
            }
        }
    }

    public static event EventHandler? TransitionChanged;

    public static IPageTransition? BuildTransition()
    {
        if (!_transitionsEnabled)
            return null;

        var duration = TimeSpan.FromMilliseconds(800);
        return new CrossFade(duration);
    }
}
