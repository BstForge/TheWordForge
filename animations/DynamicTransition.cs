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

public class DynamicTransition : IPageTransition
{
    public TimeSpan Duration { get; set; } = TimeSpan.FromMilliseconds(200);

    public async Task Start(Visual? from, Visual? to, bool forward, CancellationToken cancellationToken)
    {
        var property = ScaleTransform.ScaleYProperty;

        if (from != null)
        {
            var scale = new ScaleTransform(1, 1);
            from.RenderTransformOrigin = new RelativePoint(0.5, 0, RelativeUnit.Relative);
            from.RenderTransform = scale;
            var animOut = new Animation
            {
                Duration = Duration / 2,
                Children =
                {
                    new KeyFrame { Cue = new Cue(1), Setters = { new Setter(property, 0d) } }
                }
            };
            await animOut.RunAsync(scale, cancellationToken);
            from.IsVisible = false;
            if (from is InputElement fromElement)
                fromElement.IsHitTestVisible = false;
            from.RenderTransform = null;
        }

        if (to != null)
        {
            var start = new ScaleTransform(1, 0);
            to.RenderTransformOrigin = new RelativePoint(0.5, 0, RelativeUnit.Relative);
            to.RenderTransform = start;
            to.IsVisible = true;
            if (to is InputElement toElement)
                toElement.IsHitTestVisible = true;
            var animIn = new Animation
            {
                Duration = Duration / 2,
                Children =
                {
                    new KeyFrame { Cue = new Cue(1), Setters = { new Setter(property, 1d) } }
                }
            };
            await animIn.RunAsync(start, cancellationToken);
            to.RenderTransform = null;
        }
    }
}
