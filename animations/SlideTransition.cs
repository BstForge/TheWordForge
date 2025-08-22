using System;
using System.Threading;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Animation;
using Avalonia.Media;
using Avalonia.VisualTree;
using Avalonia.Styling;
using Avalonia.Input;

namespace TheWordForge.animations;

public class SlideTransition : IPageTransition
{
    public TimeSpan Duration { get; set; } = TimeSpan.FromMilliseconds(200);
    public PageSlide.SlideAxis Orientation { get; set; } = PageSlide.SlideAxis.Horizontal;

    public async Task Start(Visual? from, Visual? to, bool forward, CancellationToken cancellationToken)
    {
        if (cancellationToken.IsCancellationRequested)
            return;

        var parent = ((from ?? to)!).GetVisualParent()!;
        var distance = Orientation == PageSlide.SlideAxis.Horizontal ? parent.Bounds.Width : parent.Bounds.Height;
        var property = Orientation == PageSlide.SlideAxis.Horizontal ? TranslateTransform.XProperty : TranslateTransform.YProperty;

        if (from != null)
        {
            var fromTransform = new TranslateTransform();
            from.RenderTransform = fromTransform;
            var animOut = new Animation
            {
                Duration = Duration,
                Children =
                {
                    new KeyFrame { Cue = new Cue(0), Setters = { new Setter(property, 0d) } },
                    new KeyFrame { Cue = new Cue(1), Setters = { new Setter(property, forward ? -distance : distance) } }
                }
            };
            await animOut.RunAsync(fromTransform, cancellationToken);
            from.IsVisible = false;
            if (from is InputElement fromElement)
                fromElement.IsHitTestVisible = false;
            from.RenderTransform = null;
        }

        if (to != null)
        {
            var toTransform = new TranslateTransform();
            toTransform.SetValue(property, forward ? distance : -distance);
            to.RenderTransform = toTransform;
            to.IsVisible = true;
            if (to is InputElement toElement)
                toElement.IsHitTestVisible = true;
            var animIn = new Animation
            {
                Duration = Duration,
                Children =
                {
                    new KeyFrame { Cue = new Cue(0), Setters = { new Setter(property, forward ? distance : -distance) } },
                    new KeyFrame { Cue = new Cue(1), Setters = { new Setter(property, 0d) } }
                }
            };
            await animIn.RunAsync(toTransform, cancellationToken);
            to.RenderTransform = null;
        }
    }
}
