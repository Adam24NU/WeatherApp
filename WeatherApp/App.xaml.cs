namespace WeatherApp;

public partial class App : Application
{
    public App()
    {
        InitializeComponent();
        try
        {
            MainPage = new AppShell();
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