using System;
using System.Threading;
using System.Threading.Tasks;
using Avalonia.Animation;
using Avalonia.Media;
using Avalonia;
using Avalonia.VisualTree;
using Avalonia.Styling;

namespace TheWordForge.animations;

public class AccordionTransition : IPageTransition
{
    public TimeSpan Duration { get; set; } = TimeSpan.FromMilliseconds(200);
    public PageSlide.SlideAxis Orientation { get; set; } = PageSlide.SlideAxis.Vertical;

    public async Task Start(Visual? from, Visual? to, bool forward, CancellationToken cancellationToken)
    {
        var property = Orientation == PageSlide.SlideAxis.Horizontal ? ScaleTransform.ScaleXProperty : ScaleTransform.ScaleYProperty;

        if (from != null)
        {
            var scale = new ScaleTransform(1,1);
            from.RenderTransformOrigin = new RelativePoint(0.5,0.5, RelativeUnit.Relative);
            from.RenderTransform = scale;
            var animOut = new Animation
            {
                Duration = Duration/2,
                Children =
                {
                    new KeyFrame { Cue = new Cue(1), Setters = { new Setter(property, 0d) } }
                }
            };
            await animOut.RunAsync(scale, cancellationToken);
            from.IsVisible = false;
        }

        if (to != null)
        {
            var startValue = Orientation == PageSlide.SlideAxis.Horizontal ? new ScaleTransform(0,1) : new ScaleTransform(1,0);
            to.RenderTransformOrigin = new RelativePoint(0.5,0.5, RelativeUnit.Relative);
            to.RenderTransform = startValue;
            to.IsVisible = true;
            var animIn = new Animation
            {
                Duration = Duration/2,
                Children =
                {
                    new KeyFrame { Cue = new Cue(1), Setters = { new Setter(property, 1d) } }
                }
            };
            await animIn.RunAsync(startValue, cancellationToken);
        }
    }
}
