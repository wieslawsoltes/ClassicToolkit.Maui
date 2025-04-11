namespace ClassicToolkit.Maui;

internal static class RectExtensions
{
    public static Rect Deflate(this Rect rect, Thickness thickness)
    {
        return new Rect(
            new Point(rect.X + thickness.Left, rect.Y + thickness.Top),
            rect.Size.Deflate(thickness));
    }
}
