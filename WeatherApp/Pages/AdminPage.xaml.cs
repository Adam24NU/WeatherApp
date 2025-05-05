using System.Text;
using WeatherApp.Models;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using WeatherApp;
using PermissionStatus = Microsoft.Maui.ApplicationModel.PermissionStatus; // Add reference to Database

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
            try
            {
                var users = _database.GetUsers();

                StringBuilder backupContent = new StringBuilder();
                backupContent.AppendLine("User Backup - " + DateTime.Now.ToString("f"));
                backupContent.AppendLine("========================================");

                foreach (var user in users)
                {
                    backupContent.AppendLine($"ID: {user.UserId}, Email: {user.Email}, Role: {user.Role}");
                }

                string fileName = $"UserBackup_{DateTime.Now:yyyyMMdd_HHmmss}.txt";
                string filePath = Path.Combine(FileSystem.AppDataDirectory, fileName);
                File.WriteAllText(filePath, backupContent.ToString());

                // Navigate directly with data
                await Navigation.PushAsync(new UserBackupPage(users));

                LastBackupLabel.Text = $"💾 Last Backup: {DateTime.Now:f}";
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Backup failed: {ex.Message}", "OK");
            }
        }



        private string FormatBytes(long bytes)
        {
            if (bytes >= 1024 * 1024 * 1024)
                return $"{bytes / (1024.0 * 1024 * 1024):F2} GB";
            if (bytes >= 1024 * 1024)
                return $"{bytes / (1024.0 * 1024):F2} MB";
            if (bytes >= 1024)
                return $"{bytes / 1024.0:F2} KB";
            return $"{bytes} B";
        }
        private void UpdateStorageInfo()
        {
            try
            {
                //var storageService = DependencyService.Get<IStorageService>();
                var storageService = ServiceHelper.GetService<IStorageService>();
                

                if (storageService != null)
                {
                    long total = storageService.GetTotalStorageBytes();
                    long free = storageService.GetAvailableStorageBytes();
                    long used = total - free;

                    string result = $"Storage: {FormatBytes(used)} used / {FormatBytes(total)} total";
                    StorageUsageLabel.Text = result;
                }
                else
                {
                    StorageUsageLabel.Text = "Storage: Service not available";
                }
            }
            catch (Exception ex)
            {
                StorageUsageLabel.Text = $"Storage: Error - {ex.Message}";
            }
        }
        protected override async void OnAppearing()
        {
            base.OnAppearing();

            bool granted = await PermissionHelper.CheckAndRequestStoragePermissionAsync();
            if (!granted)
            {
                await DisplayAlert("Permission Required", "Storage permission is needed to fetch storage data.", "OK");
                return;
            }
            var status = await Permissions.CheckStatusAsync<Permissions.StorageRead>();
            if (status != PermissionStatus.Granted)
            {
                status = await Permissions.RequestAsync<Permissions.StorageRead>();
            }

            //return status == PermissionStatus.Granted;


            // Call your storage service now that permission is granted
            var storageService = ServiceHelper.GetService<IStorageService>();
            long available = storageService.GetAvailableStorageBytes();
            long total = storageService.GetTotalStorageBytes();

            // Display logic here
        }




    }
    // Services/IStorageService.cs
    public interface IStorageService
    {
        long GetTotalStorageBytes();
        long GetAvailableStorageBytes();
    }


    public static class PermissionHelper
    {
        public static async Task<bool> CheckAndRequestStoragePermissionAsync()
        {
            var status = await CrossPermissions.Current.CheckPermissionStatusAsync<StoragePermission>();
            if (status != Plugin.Permissions.Abstractions.PermissionStatus.Granted)
            {
                status = await CrossPermissions.Current.RequestPermissionAsync<StoragePermission>();
            }

            return status == Plugin.Permissions.Abstractions.PermissionStatus.Granted;
        }
    }

    

}
