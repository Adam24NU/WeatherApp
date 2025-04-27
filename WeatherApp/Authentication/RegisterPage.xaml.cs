using WeatherApp.Resources;  // Import the Database class

namespace WeatherApp.Authentication
{
    public partial class RegisterPage : ContentPage
    {
        private readonly Database _database;

        // Constructor to accept the Database dependency
        public RegisterPage(Database database)
        {
            InitializeComponent();
            _database = database;
        }

        // OnRegisterClicked will now use the _database field
        [Obsolete]
        private async void OnRegisterClicked(object sender, EventArgs e)
        {
            var username = UsernameEntry.Text?.Trim();
            var password = PasswordEntry.Text;
            var role = RolePicker.SelectedItem?.ToString();

            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password) || string.IsNullOrWhiteSpace(role))
            {
                StatusLabel.Text = "Please fill all fields.";
                return;
            }

            // Check for duplicates
            if (_database.RegisterUser(username, password, role))
            {
                StatusLabel.TextColor = Colors.Green;
                StatusLabel.Text = "Registration successful!";
                await Navigation.PushAsync(new LoginPage(_database));  // Navigate to LoginPage with Database
            }
            else
            {
                StatusLabel.Text = "Username already exists.";
            }
        }
    }
}
