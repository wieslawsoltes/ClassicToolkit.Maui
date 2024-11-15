# Canvas MAUI Control

[![NuGet](https://img.shields.io/nuget/v/Canvas.Maui.svg)](https://www.nuget.org/packages/Canvas.Maui)
[![NuGet](https://img.shields.io/nuget/dt/Canvas.Maui.svg)](https://www.nuget.org/packages/Canvas.Maui)

An Canvas control for MAUI.

## Usage

Add `UseCanvas()` to tour app builder.

```C#
public static MauiApp CreateMauiApp()
{
    var builder = MauiApp.CreateBuilder();
    builder
        .UseMauiApp<App>()
        .UseCanvas()
        .ConfigureFonts(fonts =>
        {
            fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
            fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
        });
#if DEBUG
    builder.Logging.AddDebug();
#endif
    return builder.Build();
}
```

Add `Canvas` control to your views.

```xaml
<?xml version="1.0" encoding="utf-8" ?>
<ContentPage xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             x:Class="MyMauiApp.MainPage">

  <Canvas WidthRequest="400" 
          HeightRequest="400" 
          Background="LightGray">
    <Label Text="Name" 
           WidthRequest="100" HeightRequest="32"
           Canvas.Left="50"
           Canvas.Top="50" />
    <Entry Text="Wiesław" 
           WidthRequest="100" HeightRequest="32"
           Canvas.Left="50"
           Canvas.Top="82" />
    <Border Background="Red"
            WidthRequest="100" HeightRequest="100"
            Canvas.Left="50"
            Canvas.Top="150">
    </Border>
  </Canvas>

</ContentPage>
```

## License

Canvas.Maui is licensed under the [MIT license](LICENSE.TXT).
