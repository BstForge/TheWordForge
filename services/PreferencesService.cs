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
    Accordion
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

    public static event EventHandler? TransitionChanged;

    public static IPageTransition? BuildTransitionFor(TransitionRegion region)
    {
        var axis = region switch
        {
            TransitionRegion.Top => PageSlide.SlideAxis.Vertical,
            _ => PageSlide.SlideAxis.Horizontal
        };

        return Transition switch
        {
            TransitionMode.Slide => new PageSlide(TimeSpan.FromMilliseconds(200), axis),
            TransitionMode.Fade => new CrossFade(TimeSpan.FromMilliseconds(200)),
            TransitionMode.Push => new PushTransition { Duration = TimeSpan.FromMilliseconds(200), Orientation = axis },
            TransitionMode.Accordion => new AccordionTransition { Duration = TimeSpan.FromMilliseconds(200), Orientation = axis },
            _ => null
        };
    }
}
