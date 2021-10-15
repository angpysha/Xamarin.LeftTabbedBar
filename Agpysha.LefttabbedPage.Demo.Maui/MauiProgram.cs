using Microsoft.Maui;
using Microsoft.Maui.Controls.Compatibility;
using Microsoft.Maui.Controls.Hosting;
using Microsoft.Maui.Hosting;

namespace Agpysha.LefttabbedPage.Demo.Maui
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
#if __ANDROID__
            Plugin.Angpysha.LeftTabbedPage.Android.LeftTabbedPageInitializer.Init(builder);
#elif __IOS__
            Plugin.Angpysha.LeftTabbedPage.iOS.LeftTabbedPageInitializer.Init(builder);
#endif
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                });

            return builder.Build();
        }
    }
}