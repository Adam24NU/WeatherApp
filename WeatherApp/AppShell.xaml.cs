using WeatherApp.Views;

namespace WeatherApp;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();

        // Navigation routes
        Routing.RegisterRoute("LoginPage", typeof(LoginPage));
        Routing.RegisterRoute("RegisterPage", typeof(RegisterPage));
        Routing.RegisterRoute("AdminPage", typeof(AdminPage));
        Routing.RegisterRoute("ScientistPage", typeof(ScientistPage));
        Routing.RegisterRoute("ScientistMapPage", typeof(ScientistMapPage));
    }
}