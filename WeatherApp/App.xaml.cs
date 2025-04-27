using Microsoft.Extensions.Configuration;  // For configuration
using System.IO;
using WeatherApp.Resources;  // Import the Database class

namespace WeatherApp;

public partial class App : Application
{
    public App()
    {
        InitializeComponent();

        try
        {
            // Load configuration from appsettings.json
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData))  // Use the correct path for app settings
                .AddJsonFile("appsettings.json")  // Make sure the file is copied to the output directory
                .Build();

            // Retrieve the connection string
            string connectionString = configuration.GetConnectionString("WeatherApp");  // Get the connection string from the config

            // Create an instance of Database (pass the connection string)
            var database = new Database(connectionString);

            // Set the MainPage to RegisterPage with the Database injected
            MainPage = new NavigationPage(new Authentication.RegisterPage(database));
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"App init error: {ex}");
            MainPage = new ContentPage
            {
                Content = new Label { Text = $"Error: {ex.Message}", VerticalOptions = LayoutOptions.Center }
            };
        }

        // Global exception handlers
        AppDomain.CurrentDomain.UnhandledException += (s, e) =>
        {
            System.Diagnostics.Debug.WriteLine($"Unhandled exception: {e.ExceptionObject}");
        };
        TaskScheduler.UnobservedTaskException += (s, e) =>
        {
            System.Diagnostics.Debug.WriteLine($"Unobserved task exception: {e.Exception}");
            e.SetObserved();
        };
    }
}