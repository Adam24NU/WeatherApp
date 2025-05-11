using WeatherApp.Authentication;  // Assuming this is where your pages are
using Microsoft.Extensions.Logging;  // Ensure this is included at the top
using WeatherApp.Pages;

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

        // Register the SQL Server connection
        builder.Services.AddSingleton<Database>();  // Register Database

        // Register your pages (adjust to match your project structure)
        builder.Services.AddTransient<RegisterPage>();  
        builder.Services.AddTransient<LoginPage>();     
        builder.Services.AddTransient<AdminPage>();    

        return builder.Build();
    }
}