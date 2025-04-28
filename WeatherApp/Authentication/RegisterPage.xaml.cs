using WeatherApp;
using WeatherApp.Models;

namespace WeatherApp.Authentication
{
    public partial class RegisterPage : ContentPage
    {
        private readonly Database _database;

        // Constructor to accept the Database dependency
        public RegisterPage(Database database)
        {
            InitializeComponent();
            _database = database; // Using the passed-in Database object instead of creating a new one
        }

        // OnRegisterClicked will now use the _database field
        private async void OnRegisterClicked(object sender, EventArgs e)
        {
            var email = EmailEntry.Text?.Trim();
            var password = PasswordEntry.Text;
            var role = RolePicker.SelectedItem?.ToString();

            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password) || string.IsNullOrWhiteSpace(role))
            {
                StatusLabel.Text = "Please fill all fields.";
                return;
            }

            // Register user in the database
            if (_database.RegisterUser(email, password, role))
            {
                StatusLabel.TextColor = Colors.Green;
                StatusLabel.Text = "Registration successful!";
                await Navigation.PushAsync(new LoginPage(_database));  // Navigate to LoginPage and pass the Database object
            }
            else
            {
                StatusLabel.Text = "Email already exists.";
            }
        }
        // Navigate to LoginPage
        private async void OnLoginClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new LoginPage(_database));  // Navigate to the LoginPage
        }
    }
}