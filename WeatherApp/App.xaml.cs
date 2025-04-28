using Microsoft.Extensions.Configuration;  // For configuration
using System.IO;
using WeatherApp;


namespace WeatherApp
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            try
            {
                // Hardcoding the connection string directly in the code
                string connectionString =
                    "Server=10.0.2.2,1433,1433;Database=WeatherApp;User Id=sa;Password=<YourPassw0rd>;TrustServerCertificate=True;";

                // Create an instance of Database with the hardcoded connection string
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
}