using WeatherApp.Authentication;  // Assuming this is where your pages are
using Microsoft.Extensions.Logging;  // Ensure this is included at the top
using WeatherApp.Pages;
using WeatherApp.Services;

#if ANDROID
using WeatherApp.Platforms.Android;
#endif

namespace WeatherApp;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

#if DEBUG
        builder.Logging.AddDebug();
#endif
        
#if ANDROID
        builder.Services.AddSingleton<WeatherApp.Services.IStorageService, StorageService_Android>();
#endif

        // Register the SQL Server connection
        builder.Services.AddSingleton<Database>();  // Register Database

        // Register your pages (adjust to match your project structure)
        builder.Services.AddTransient<RegisterPage>();  // Ensure your RegisterPage is here
        builder.Services.AddTransient<LoginPage>();     // Likewise, ensure other pages are included if needed
        builder.Services.AddTransient<AdminPage>();     // Example for AdminPage if required

        return builder.Build();
    }
}