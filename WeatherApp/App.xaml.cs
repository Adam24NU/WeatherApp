using Microsoft.Extensions.Configuration;
using Microsoft.Maui.Controls;
using WeatherApp.Resources;  // Import the Database class
using System.IO;

namespace WeatherApp;

public partial class App : Application
{
    public App()
    {
        InitializeComponent();

        // Define the path where appsettings.json should be located (LocalAppData for mobile platforms)
        var appDataDirectory = FileSystem.AppDataDirectory;

        // Load configuration from appsettings.json (ensure this file is included in the output directory)
        var configuration = new ConfigurationBuilder()
            .SetBasePath(appDataDirectory)  // Use AppDataDirectory to ensure correct location on Android
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();

        // Retrieve the connection string from appsettings.json
        string connectionString = configuration.GetConnectionString("WeatherApp");

        // Create an instance of Database (pass the connection string)
        var database = new Database(connectionString);

        // Set the MainPage to RegisterPage with the Database injected
        MainPage = new NavigationPage(new Authentication.RegisterPage(database));
    }
}