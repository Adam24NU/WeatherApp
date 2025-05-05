namespace WeatherApp;

public partial class AppShell : Shell
{
	public AppShell()
	{
		InitializeComponent();

		// Register route for the backup display page
		Routing.RegisterRoute("UserBackupPage", typeof(WeatherApp.Pages.UserBackupPage));
	}
}



