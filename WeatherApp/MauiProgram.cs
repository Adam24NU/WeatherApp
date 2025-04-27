using Microsoft.Extensions.Logging;
using WeatherApp.Pages;
using WeatherApp.Authentication;
using WeatherApp.Models;


namespace WeatherApp
{

    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseMauiMaps()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

#if DEBUG
            builder.Logging.AddDebug();
#endif

            // Register the SQL Server connection (Connection string from config)
            builder.Services.AddSingleton<appsettings>(); // Register as singleton


            // Register ViewModels and Views
            builder.Services.AddTransient<MainPage>();
            builder.Services.AddTransient<LoginPage>();
            builder.Services.AddTransient<RegisterPage>();

            return builder.Build();
        }
    }
}