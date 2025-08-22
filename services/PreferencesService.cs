using System;
using Avalonia.Animation;
using Avalonia.Animation.Easings;
using Avalonia;
using TheWordForge.animations;

namespace TheWordForge.services;

public enum TransitionMode
{
    Off,
    Slide,
    Fade,
    Push,
    Dynamic
}

public enum TransitionSpeed
{
    Fast,
    Slow
}

public enum TransitionRegion
{
    Top,
    Left,
    Center
}

public static class PreferencesService
{
    private static TransitionMode _transition = TransitionMode.Off;
    private static TransitionSpeed _speed = TransitionSpeed.Fast;

    public static TransitionMode Transition
    {
        get => _transition;
        set
        {
            if (_transition != value)
            {
                _transition = value;
                TransitionChanged?.Invoke(null, EventArgs.Empty);
            }
        }
    }

    public static TransitionSpeed Speed
    {
        get => _speed;
        set
        {
            if (_speed != value)
            {
                _speed = value;
                TransitionChanged?.Invoke(null, EventArgs.Empty);
            }
        }
    }

    public static event EventHandler? TransitionChanged;

    public static IPageTransition? BuildTransitionFor(TransitionRegion region)
    {
        if (region == TransitionRegion.Left)
            return null;

        var axis = region switch
        {
            TransitionRegion.Top => PageSlide.SlideAxis.Vertical,
            _ => PageSlide.SlideAxis.Horizontal
        };

        var duration = Speed == TransitionSpeed.Fast
            ? TimeSpan.FromMilliseconds(200)
            : TimeSpan.FromMilliseconds(400);

        return Transition switch
        {
            TransitionMode.Slide => new SlideTransition { Duration = duration, Orientation = axis },
            TransitionMode.Fade => new CrossFade(duration),
            TransitionMode.Push => new PushTransition { Duration = duration, Orientation = axis },
            TransitionMode.Dynamic => new DynamicTransition { Duration = duration },
            _ => null
        };
    }
}
