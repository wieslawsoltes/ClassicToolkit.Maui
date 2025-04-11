using Microsoft.Maui.Layouts;
using ILayout = Microsoft.Maui.ILayout;

namespace ClassicToolkit.Maui;

/// <summary>
/// Base class for controls which decorate a single child control.
/// </summary>
[ContentProperty(nameof(Child))]
public class Decorator : Layout, ILayoutManager
{
    public static readonly BindableProperty ChildProperty =
        BindableProperty.Create(
            nameof(Child),
            typeof(IView),
            typeof(Viewbox),
            defaultValue: null,
            propertyChanged: ChildPropertyChanged);

    public IView Child
    {
        get => (IView)GetValue(ChildProperty);
        set => SetValue(ChildProperty, value);
    }

    private static void ChildPropertyChanged(BindableObject bindableObject, object oldValue, object newValue)
    {
        if (bindableObject is Decorator decorator)
        {
            if (oldValue is IView oldChild)
            {
                decorator.Children.Remove(oldChild);
            }

            if (newValue is IView newChild)
            {
                decorator.Children.Add(newChild);
            }
        }
    }
    
    protected override ILayoutManager CreateLayoutManager()
    {
        return this;
    }

    public Size Measure(double widthConstraint, double heightConstraint)
    {
        var padding = Padding;

        if (Child is null)
        {
            return new Size().Inflate(padding);
        }

        var availableSize = new Size(widthConstraint, heightConstraint);

        // TODO:
        // (widthConstraint, heightConstraint) = availableSize.Deflate(padding);
        (widthConstraint, heightConstraint) = availableSize;
            
        Child.Measure(widthConstraint, heightConstraint);

        var (measuredWidth, measuredHeight) = Child.DesiredSize.Inflate(padding);

        var finalHeight = LayoutManager.ResolveConstraints(
            heightConstraint, 
            Child.Height, 
            measuredHeight,
            ((ILayout)this).MinimumHeight, 
            ((ILayout)this).MaximumHeight);

        var finalWidth = LayoutManager.ResolveConstraints(
            widthConstraint, 
            Child.Width, 
            measuredWidth,
            ((ILayout)this).MinimumWidth, 
            ((ILayout)this).MaximumWidth);

        return new Size(finalWidth, finalHeight);
    }

    public Size ArrangeChildren(Rect bounds)
    {
        var padding = Padding;

        double width = bounds.Width - padding.HorizontalThickness;
        double height = bounds.Height - padding.VerticalThickness;

        var actual = new Size(width, height);

        var finalSize = actual.AdjustForFill(bounds, this);

        if (Child is not null)
        {
            Child.Arrange(new Rect(new Point(0, 0), finalSize).Deflate(padding));
        }

        return finalSize;
    }
}

