using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Animation;
using Avalonia.Media;
using Avalonia.VisualTree;
using Avalonia.Styling;
using Avalonia.Input;

namespace TheWordForge.animations;

public class PushTransition : IPageTransition
{
    public TimeSpan Duration { get; set; } = TimeSpan.FromMilliseconds(200);
    public PageSlide.SlideAxis Orientation { get; set; } = PageSlide.SlideAxis.Horizontal;

    public async Task Start(Visual? from, Visual? to, bool forward, CancellationToken cancellationToken)
    {
        if (cancellationToken.IsCancellationRequested)
            return;

        var tasks = new List<Task>();
        var parent = ((from ?? to)!).GetVisualParent()!;
        var distance = Orientation == PageSlide.SlideAxis.Horizontal ? parent.Bounds.Width : parent.Bounds.Height;
        var property = Orientation == PageSlide.SlideAxis.Horizontal ? TranslateTransform.XProperty : TranslateTransform.YProperty;

        if (from != null)
        {
            var transform = new TranslateTransform();
            from.RenderTransform = transform;
            var anim = new Animation
            {
                Duration = Duration,
                Children =
                {
                    new KeyFrame { Cue = new Cue(0), Setters = { new Setter(property, 0d) } },
                    new KeyFrame { Cue = new Cue(1), Setters = { new Setter(property, forward ? distance : -distance) } }
                }
            };
            tasks.Add(anim.RunAsync(transform, cancellationToken));
        }

        if (to != null)
        {
            var transform = new TranslateTransform();
            transform.SetValue(property, forward ? -distance : distance);
            to.RenderTransform = transform;
            to.IsVisible = true;
            if (to is InputElement toElement)
                toElement.IsHitTestVisible = true;
            var anim = new Animation
            {
                Duration = Duration,
                Children =
                {
                    new KeyFrame { Cue = new Cue(0), Setters = { new Setter(property, forward ? -distance : distance) } },
                    new KeyFrame { Cue = new Cue(1), Setters = { new Setter(property, 0d) } }
                }
            };
            tasks.Add(anim.RunAsync(transform, cancellationToken));
        }

        await Task.WhenAll(tasks);

        if (from != null && !cancellationToken.IsCancellationRequested)
        {
            from.IsVisible = false;
            if (from is InputElement fromElement)
                fromElement.IsHitTestVisible = false;
            from.RenderTransform = null;
        }

        if (to != null)
            to.RenderTransform = null;
    }
}
