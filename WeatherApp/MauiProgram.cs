using Microsoft.Extensions.Logging;
using WeatherApp.Repositories;
using WeatherApp.Tools;
using WeatherApp.ViewModels;
using WeatherApp.Views;

namespace WeatherApp;

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

        // Register the SQL Server connection
        builder.Services.AddSingleton<DatabaseConnection>();

        // Register repositories
        builder.Services.AddSingleton<UserRepository>();
        builder.Services.AddSingleton<SiteRepository>();
        builder.Services.AddSingleton<SensorRepository>();
        builder.Services.AddSingleton<PhysicalQuantityRepository>();
        builder.Services.AddSingleton<ConfigSettingRepository>();
        builder.Services.AddSingleton<IncidentRepository>();
        builder.Services.AddSingleton<IncidentMeasurementRepository>();
        builder.Services.AddSingleton<MeasurementRepository>();
        builder.Services.AddSingleton<MaintenanceRepository>();
        builder.Services.AddSingleton<UserRepository>();
        builder.Services.AddSingleton<SensorRepository>();
        builder.Services.AddSingleton<PhysicalQuantityRepository>();
        builder.Services.AddSingleton<ConfigSettingRepository>();
        builder.Services.AddSingleton<IncidentRepository>();
        builder.Services.AddSingleton<IncidentMeasurementRepository>();
        builder.Services.AddSingleton<MeasurementRepository>();
        builder.Services.AddSingleton<MaintenanceRepository>();

        // Register ViewModels and Views
        builder.Services.AddTransient<MainPageViewModel>();
        builder.Services.AddTransient<MainPage>();
        builder.Services.AddTransient<LoginPageViewModel>();
        builder.Services.AddTransient<LoginPage>();
        builder.Services.AddTransient<RegisterPageViewModel>();
        builder.Services.AddTransient<RegisterPage>();
        builder.Services.AddTransient<AdminPageViewModel>();
        builder.Services.AddTransient<AdminPage>();
        builder.Services.AddTransient<ScientistPageViewModel>();
        builder.Services.AddTransient<ScientistPage>();
        builder.Services.AddTransient<ScientistMapPageViewModel>();
        builder.Services.AddTransient<ScientistMapPage>();
        builder.Services.AddTransient<ISiteRepository, SiteRepository>();
        builder.Services.AddTransient<IMapInvoker, ScientistMapPage>();
        builder.Services.AddTransient<OpsManagerPage>();

        return builder.Build();
    }
}

