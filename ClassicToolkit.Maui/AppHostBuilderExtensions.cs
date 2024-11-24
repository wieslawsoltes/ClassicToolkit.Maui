namespace ClassicToolkit.Maui;

public static class AppHostBuilderExtensions
{
    public static MauiAppBuilder UseClassicToolkit(this MauiAppBuilder builder)
    {
        GC.KeepAlive(typeof(Canvas));
        GC.KeepAlive(typeof(Decorator));
        GC.KeepAlive(typeof(Viewbox));
        return builder;
    }
}
