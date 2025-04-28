using WeatherApp.Models;
using WeatherApp;  // Add reference to Database

namespace WeatherApp.Pages
{
    public partial class AdminPage : ContentPage
    {
        private readonly Database _database;  // Database instance to interact with DB

        // Constructor to inject Database
        public AdminPage(Database database)
        {
            InitializeComponent();
            _database = database;
            LoadUsers();  // Load users from the database
            LastBackupLabel.Text = $"💾 Last Backup: {DateTime.Now:f}";
        }

        // Method to load users from the database and bind them to the UserListView
        private void LoadUsers()
        {
            var users = _database.GetUsers(); // Method to get users from the database
            UserListView.ItemsSource = users;
        }

        // Create user button click handler
        private async void OnCreateUserClicked(object sender, EventArgs e)
        {
            string email = NewUsernameEntry.Text?.Trim();  // Treating email as the username
            string password = NewPasswordEntry.Text;
            string role = RolePicker.SelectedItem?.ToString();

            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password) || string.IsNullOrWhiteSpace(role))
            {
                await DisplayAlert("Error", "Please fill out all fields.", "OK");
                return;
            }

            // Register the user in the database with email as username
            bool isUserCreated = _database.RegisterUser(email, password, role);

            if (!isUserCreated)
            {
                await DisplayAlert("Error", "Username already exists.", "OK");
                return;
            }

            // Clear inputs
            NewUsernameEntry.Text = "";
            NewPasswordEntry.Text = "";
            RolePicker.SelectedIndex = -1;

            // Reload user list
            LoadUsers();

            await DisplayAlert("Success", "User created successfully!", "OK");
        }

        // Delete user button click handler
        private async void OnDeleteUserClicked(object sender, EventArgs e)
        {
            var selectedUser = UserListView.SelectedItem as User;
            if (selectedUser == null)
            {
                await DisplayAlert("Error", "Please select a user to delete.", "OK");
                return;
            }

            // Delete user from the database
            bool isDeleted = _database.DeleteUser(selectedUser.UserId);

            if (isDeleted)
            {
                // Reload user list after deletion
                LoadUsers();
                await DisplayAlert("Deleted", "User has been removed.", "OK");
            }
            else
            {
                await DisplayAlert("Error", "Unable to delete user.", "OK");
            }
        }

        // Simulate firmware update action (still optional in this case)
        private async void OnFirmwareUpdateClicked(object sender, EventArgs e)
        {
            await DisplayAlert("Firmware Update", "Sensor firmware update pushed successfully!", "OK");
        }
        // Backup Data button handler (simulating backup)
        private async void OnBackupDataClicked(object sender, EventArgs e)
        {
            // Logic to simulate backup data
            await DisplayAlert("Backup", "Data backup completed successfully!", "OK");

            // For now, simulate updating backup timestamp
            LastBackupLabel.Text = $"💾 Last Backup: {DateTime.Now:f}";
        }
    }
}
