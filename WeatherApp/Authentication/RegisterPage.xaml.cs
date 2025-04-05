using WeatherApp.Models;

namespace WeatherApp.Authentication
{
    public partial class RegisterPage : ContentPage
    {
        // Mock in-memory user list (you can move this to a shared service later)
        public static List<User> RegisteredUsers = new();

        public RegisterPage()
        {
            InitializeComponent();
        }
        [Obsolete]
        private async void OnRegisterClicked(object sender, EventArgs e)
        {
            var username = UsernameEntry.Text?.Trim();
            var password = PasswordEntry.Text;
            var role = RolePicker.SelectedItem?.ToString();

            if (string.IsNullOrWhiteSpace(username) ||
                string.IsNullOrWhiteSpace(password) ||
                string.IsNullOrWhiteSpace(role))
            {
                StatusLabel.Text = "Please fill all fields.";
                return;
            }

            // Check for duplicates
            if (RegisteredUsers.Any(u => u.Username == username))
            {
                StatusLabel.Text = "Username already exists.";
                return;
            }

            UserStore.RegisteredUsers.Add(new User
            {
                Username = username,
                Password = password,
                Role = role
            });

            StatusLabel.TextColor = Colors.Green;
            StatusLabel.Text = "Registration successful!";

            // Optional: Navigate to login page automatically
            await Navigation.PushAsync(new LoginPage());
        }
    }
}
