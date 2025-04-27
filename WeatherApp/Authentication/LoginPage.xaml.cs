using WeatherApp.Resources;  // Add reference to Database
using WeatherApp.Models;  // Add reference to User model
using WeatherApp.Pages;  // Add reference to the pages
using WeatherApp;

namespace WeatherApp.Authentication
{
    public partial class LoginPage : ContentPage
    {
        private readonly Database _database;  // Adam's task: Database instance

        // Adam's task: Constructor to inject Database
        public LoginPage(Database database)
        {
            InitializeComponent();
            _database = database;
        }

        [Obsolete]
        private async void OnLoginClicked(object sender, EventArgs e)
        {
            var username = UsernameEntry.Text?.Trim();
            var password = PasswordEntry.Text;

            // Adam's task: Authenticate user against the database
            var user = _database.AuthenticateUser(username, password);

            if (user == null)
            {
                StatusLabel.Text = "Invalid username or password.";  // Display error
                return;
            }

            // ✅ Navigate based on role (Adam's task: role-based navigation)
            switch (user.Role)
            {
                case "Scientist":
                    await Navigation.PushAsync(new MainPage());
                    break;
                case "Administrator":
                    await Navigation.PushAsync(new AdminPage(_database)); // Pass the database
                    break;
                case "Operations Manager":
                    await Navigation.PushAsync(new OpsManagerPage(_database)); // Pass the database
                    break;
            }
        }
    }
}
