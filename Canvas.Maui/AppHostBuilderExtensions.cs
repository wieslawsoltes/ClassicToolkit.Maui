namespace Canvas.Maui;

public static class AppHostBuilderExtensions
{
    public static MauiAppBuilder UseCanvas(this MauiAppBuilder builder)
    {
        GC.KeepAlive(typeof(Canvas));
        return builder;
    }
}
