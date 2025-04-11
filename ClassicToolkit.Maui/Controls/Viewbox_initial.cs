using Microsoft.Maui.Layouts;

namespace ClassicToolkit.Maui;

/*
public class Viewbox : Layout, ILayoutManager
{
    public static readonly BindableProperty StretchProperty =
        BindableProperty.Create(nameof(Stretch), typeof(Stretch), typeof(Viewbox), Microsoft.Maui.Controls.Stretch.Uniform,
            propertyChanged: (bindable, oldValue, newValue) => ((Viewbox)bindable).InvalidateMeasure());

    public Stretch Stretch
    {
        get => (Stretch)GetValue(StretchProperty);
        set => SetValue(StretchProperty, value);
    }

    public Viewbox()
    {
        Handler?.DisconnectHandler();
    }

    protected override ILayoutManager CreateLayoutManager()
    {
        return this;
    }

    Size ILayoutManager.Measure(double widthConstraint, double heightConstraint)
    {
        var content = Children.FirstOrDefault();
        if (content == null)
            return new Size(0, 0);

        // Get the child's desired size
        var contentSize = content.Measure(double.PositiveInfinity, double.PositiveInfinity);

        // Calculate the size based on stretch mode and constraints
        switch (Stretch)
        {
            case Stretch.None:
                return contentSize;

            case Stretch.Fill:
                return new Size(
                    Math.Min(widthConstraint, contentSize.Width),
                    Math.Min(heightConstraint, contentSize.Height));

            case Stretch.Uniform:
            case Stretch.UniformToFill:
                if (contentSize.Width == 0 || contentSize.Height == 0)
                    return new Size(0, 0);

                var scaleX = widthConstraint / contentSize.Width;
                var scaleY = heightConstraint / contentSize.Height;

                var scale = Stretch == Stretch.Uniform
                    ? Math.Min(scaleX, scaleY)
                    : Math.Max(scaleX, scaleY);

                return new Size(
                    Math.Min(contentSize.Width * scale, widthConstraint),
                    Math.Min(contentSize.Height * scale, heightConstraint));

            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    Size ILayoutManager.ArrangeChildren(Rect bounds)
    {
        var content = Children.FirstOrDefault();
        if (content == null)
            return bounds.Size;

        var contentDesiredSize = content.Measure(double.PositiveInfinity, double.PositiveInfinity);

        if (contentDesiredSize.Width == 0 || contentDesiredSize.Height == 0)
            return bounds.Size;

        double scaleX = bounds.Width / contentDesiredSize.Width;
        double scaleY = bounds.Height / contentDesiredSize.Height;
        double scale = 1;

        switch (Stretch)
        {
            case Stretch.None:
                scale = 1;
                break;

            case Stretch.Fill:
                // Content stretches to fill the available space
                content.Arrange(bounds);
                return bounds.Size;

            case Stretch.Uniform:
                scale = Math.Min(scaleX, scaleY);
                break;

            case Stretch.UniformToFill:
                scale = Math.Max(scaleX, scaleY);
                break;
        }

        // Calculate centered position
        var scaledWidth = contentDesiredSize.Width * scale;
        var scaledHeight = contentDesiredSize.Height * scale;
        var x = bounds.X + (bounds.Width - scaledWidth) / 2;
        var y = bounds.Y + (bounds.Height - scaledHeight) / 2;

        // Arrange the content
        content.Arrange(new Rect(x, y, scaledWidth, scaledHeight));

        if (content is VisualElement visualElement)
        {
            visualElement.ScaleX = scale;
            visualElement.ScaleY = scale;
        }

        return bounds.Size;
    }
}
*/
