using Microsoft.Extensions.Configuration;
using Microsoft.Maui.Controls.Hosting;
using Microsoft.Maui.Hosting;
using System.IO;

namespace WeatherApp
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();

            // Load the configuration file from the Resources/raw folder for Android
            builder.Configuration
                .SetBasePath(FileSystem.AppDataDirectory) // This is for ensuring that the base path is set correctly for app data
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true); // Load the appsettings.json from raw folder

            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

            return builder.Build();
        }
    }
}
