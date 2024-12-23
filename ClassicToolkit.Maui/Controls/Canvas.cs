// Based on https://github.com/AvaloniaUI/Avalonia/blob/a5f5b8f67ec0cd2fdde444ea7ba0c8cdd6d8d048/src/Avalonia.Controls/Canvas.cs
using Microsoft.Maui.Layouts;
using ILayout = Microsoft.Maui.ILayout;

namespace ClassicToolkit.Maui;

/// <summary>
/// A panel that displays child controls at arbitrary locations.
/// </summary>
/// <remarks>
/// Unlike other <see cref="Layout"/> implementations, the <see cref="Canvas"/> doesn't lay out
/// its children in any particular layout. Instead, the positioning of each child control is
/// defined by the <code>Canvas.Left</code>, <code>Canvas.Top</code>, <code>Canvas.Right</code>
/// and <code>Canvas.Bottom</code> attached properties.
/// </remarks>
[ContentProperty(nameof(Children))]
public class Canvas : Layout, ILayoutManager
{
    protected override ILayoutManager CreateLayoutManager() => this;

    /// <summary>
    /// Defines the Left attached property.
    /// </summary>
    public static readonly BindableProperty LeftProperty = 
        BindableProperty.CreateAttached("Left", typeof(double), typeof(Canvas), double.NaN);

    /// <summary>
    /// Defines the Top attached property.
    /// </summary>
    public static readonly BindableProperty TopProperty = 
        BindableProperty.CreateAttached("Top", typeof(double), typeof(Canvas), double.NaN);

    /// <summary>
    /// Defines the Right attached property.
    /// </summary>
    public static readonly BindableProperty RightProperty = 
        BindableProperty.CreateAttached("Right", typeof(double), typeof(Canvas), double.NaN);

    /// <summary>
    /// Defines the Bottom attached property.
    /// </summary>
    public static readonly BindableProperty BottomProperty = 
        BindableProperty.CreateAttached("Bottom", typeof(double), typeof(Canvas), double.NaN);

    /// <summary>
    /// Initializes static members of the <see cref="Canvas"/> class.
    /// </summary>
    static Canvas()
    {
        // ClipToBoundsProperty.OverrideDefaultValue<Canvas>(false);
        // AffectsParentArrange<Canvas>(LeftProperty, TopProperty, RightProperty, BottomProperty);
    }

    /// <summary>
    /// Gets the value of the Left attached property for a control.
    /// </summary>
    /// <param name="element">The control.</param>
    /// <returns>The control's left coordinate.</returns>
    public static double GetLeft(BindableObject element)
    {
        return (double)element.GetValue(LeftProperty);
    }

    /// <summary>
    /// Sets the value of the Left attached property for a control.
    /// </summary>
    /// <param name="element">The control.</param>
    /// <param name="value">The left value.</param>
    public static void SetLeft(BindableObject element, double value)
    {
        element.SetValue(LeftProperty, value);
    }

    /// <summary>
    /// Gets the value of the Top attached property for a control.
    /// </summary>
    /// <param name="element">The control.</param>
    /// <returns>The control's top coordinate.</returns>
    public static double GetTop(BindableObject element)
    {
        return (double)element.GetValue(TopProperty);
    }

    /// <summary>
    /// Sets the value of the Top attached property for a control.
    /// </summary>
    /// <param name="element">The control.</param>
    /// <param name="value">The top value.</param>
    public static void SetTop(BindableObject element, double value)
    {
        element.SetValue(TopProperty, value);
    }

    /// <summary>
    /// Gets the value of the Right attached property for a control.
    /// </summary>
    /// <param name="element">The control.</param>
    /// <returns>The control's right coordinate.</returns>
    public static double GetRight(BindableObject element)
    {
        return (double)element.GetValue(RightProperty);
    }

    /// <summary>
    /// Sets the value of the Right attached property for a control.
    /// </summary>
    /// <param name="element">The control.</param>
    /// <param name="value">The right value.</param>
    public static void SetRight(BindableObject element, double value)
    {
        element.SetValue(RightProperty, value);
    }

    /// <summary>
    /// Gets the value of the Bottom attached property for a control.
    /// </summary>
    /// <param name="element">The control.</param>
    /// <returns>The control's bottom coordinate.</returns>
    public static double GetBottom(BindableObject element)
    {
        return (double)element.GetValue(BottomProperty);
    }

    /// <summary>
    /// Sets the value of the Bottom attached property for a control.
    /// </summary>
    /// <param name="element">The control.</param>
    /// <param name="value">The bottom value.</param>
    public static void SetBottom(BindableObject element, double value)
    {
        element.SetValue(BottomProperty, value);
    }
    
    Size ILayoutManager.Measure(double widthConstraint, double heightConstraint)
    {
        var padding = Padding;

        for (int n = 0; n < Count; n++)
        {
            var child = this[n];

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

        var finalHeight = LayoutManager.ResolveConstraints(
            heightConstraint, 
            Height, 
            measuredHeight,
            ((ILayout)this).MinimumHeight, 
            ((ILayout)this).MaximumHeight);

        var finalWidth = LayoutManager.ResolveConstraints(
            widthConstraint, 
            Width, 
            measuredWidth,
            ((ILayout)this).MinimumWidth, 
            ((ILayout)this).MaximumWidth);

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

    Size ILayoutManager.ArrangeChildren(Rect bounds)
    {
        var padding = Padding;

        double width = bounds.Width - padding.HorizontalThickness;
        double height = bounds.Height - padding.VerticalThickness;

        var actual = new Size(width, height);

        var finalSize = actual.AdjustForFill(bounds, this);
        
        for (int n = 0; n < Count; n++)
        {
            var child = this[n];

            if (child.Visibility == Visibility.Collapsed)
            {
                continue;
            }

            ArrangeChild(child, finalSize);

        }

        return finalSize;
    }
}
