namespace ClassicToolkit.Maui;

internal static class SizeExtensions
{
    public static Size Deflate(this Size size, Thickness thickness)
    {
        return new Size(
            Math.Max(0, size.Width - thickness.Left - thickness.Right),
            Math.Max(0, size.Height - thickness.Top - thickness.Bottom));
    }

    public static Size Inflate(this Size size, Thickness thickness)
    {
        return new Size(
            size.Width + thickness.Left + thickness.Right,
            size.Height + thickness.Top + thickness.Bottom);
    }
}
