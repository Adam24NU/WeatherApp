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

        // Load configuration from appsettings.json
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData))
            .AddJsonFile("appsettings.json")
            .Build();



        // Retrieve the connection string
        string connectionString = configuration.GetConnectionString("WeatherAppDb");

        // Create an instance of Database (pass the connection string)
        var database = new Database(connectionString);

        // Set the MainPage to RegisterPage with the Database injected
        MainPage = new NavigationPage(new Authentication.RegisterPage(database));
    }
}

