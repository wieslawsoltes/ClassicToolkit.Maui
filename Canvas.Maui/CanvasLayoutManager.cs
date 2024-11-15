// Based on https://github.com/AvaloniaUI/Avalonia/blob/a5f5b8f67ec0cd2fdde444ea7ba0c8cdd6d8d048/src/Avalonia.Controls/Canvas.cs
using Microsoft.Maui.Layouts;
using ILayout = Microsoft.Maui.ILayout;

namespace Canvas.Maui;

public class CanvasLayoutManager(Canvas layout) : LayoutManager(layout)
{
    public Canvas Canvas { get; } = layout;

    public override Size Measure(double widthConstraint, double heightConstraint)
    {
        var padding = Canvas.Padding;

        for (int n = 0; n < Canvas.Count; n++)
        {
            var child = Canvas[n];

            if (child.Visibility == Visibility.Collapsed)
            {
                continue;
            }

            child.Measure(double.PositiveInfinity, double.PositiveInfinity);
        }

        double measuredHeight = 0;
        double measuredWidth = 0;

        measuredHeight += padding.VerticalThickness;
        measuredWidth += padding.HorizontalThickness;

        var finalHeight = ResolveConstraints(heightConstraint, Canvas.Height, measuredHeight, ((ILayout)Canvas).MinimumHeight, ((ILayout)Canvas).MaximumHeight);
        var finalWidth = ResolveConstraints(widthConstraint, Canvas.Width, measuredWidth, ((ILayout)Canvas).MinimumWidth, ((ILayout)Canvas).MaximumWidth);

        return new Size(finalWidth, finalHeight);
    }

    protected virtual void ArrangeChild(IView child, Size finalSize)
    {
        double x = 0.0;
        double y = 0.0;
        double elementLeft = Canvas.GetLeft((BindableObject)child);

        if (!double.IsNaN(elementLeft))
        {
            x = elementLeft;
        }
        else
        {
            // Arrange with right.
            double elementRight = Canvas.GetRight((BindableObject)child);
            if (!double.IsNaN(elementRight))
            {
                x = finalSize.Width - child.DesiredSize.Width - elementRight;
            }
        }

        double elementTop = Canvas.GetTop((BindableObject)child);
        if (!double.IsNaN(elementTop))
        {
            y = elementTop;
        }
        else
        {
            double elementBottom = Canvas.GetBottom((BindableObject)child);
            if (!double.IsNaN(elementBottom))
            {
                y = finalSize.Height - child.DesiredSize.Height - elementBottom;
            }
        }

        child.Arrange(new Rect(new Point(x, y), child.DesiredSize));
    }
    
    public override Size ArrangeChildren(Rect bounds)
    {
        var padding = Canvas.Padding;

        double width = bounds.Width - padding.HorizontalThickness;
        double height = bounds.Height - padding.VerticalThickness;

        var actual = new Size(width, height);

        var finalSize = actual.AdjustForFill(bounds, Canvas);
        
        for (int n = 0; n < Canvas.Count; n++)
        {
            var child = Canvas[n];

            if (child.Visibility == Visibility.Collapsed)
            {
                continue;
            }

            ArrangeChild(child, finalSize);

        }

        return finalSize;
    }
}
