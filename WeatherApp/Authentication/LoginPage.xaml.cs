using WeatherApp.Models;  // Add reference to User model
using WeatherApp.Pages;  // Add reference to the pages
using WeatherApp;

namespace WeatherApp.Authentication
{
    public partial class LoginPage : ContentPage
    {
        private readonly Database _database;  // Database instance

        // Constructor to inject Database
        public LoginPage(Database database)
        {
            InitializeComponent();
            _database = database;
        }

        [Obsolete]
        private async void OnLoginClicked(object sender, EventArgs e)
        {
            var email = EmailEntry.Text?.Trim();  // Changed to email
            var password = PasswordEntry.Text;

            // Authenticate user against the database using email
            var user = _database.AuthenticateUser(email, password);

            if (user == null)
            {
                StatusLabel.Text = "Invalid email or password.";  // Display error
                return;
            }

            // Navigate based on role
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
                default:
                    StatusLabel.Text = "Role not found.";
                    break;
            }
        }
    }
}