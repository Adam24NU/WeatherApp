using System;
using Microsoft.Maui.Controls;

namespace WeatherApp.Pages
{
    public partial class AdminPage : ContentPage
    {
        public AdminPage()
        {
            InitializeComponent();
            UserListView.ItemsSource = UserStore.RegisteredUsers;
            LastBackupLabel.Text = $"💾 Last Backup: {DateTime.Now:f}";
        }

        private void OnCreateUserClicked(object sender, EventArgs e)
        {
            string username = NewUsernameEntry.Text?.Trim();
            string password = NewPasswordEntry.Text;
            string role = RolePicker.SelectedItem?.ToString();

            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password) || string.IsNullOrWhiteSpace(role))
            {
                DisplayAlert("Error", "Please fill out all fields.", "OK");
                return;
            }

            // Check if username already exists
            if (UserStore.RegisteredUsers.Any(u => u.Username.Equals(username, StringComparison.OrdinalIgnoreCase)))
            {
                DisplayAlert("Error", "Username already exists.", "OK");
                return;
            }

            UserStore.RegisteredUsers.Add(new User
            {
                Username = username,
                Password = password,
                Role = role
            });

            NewUsernameEntry.Text = "";
            NewPasswordEntry.Text = "";
            RolePicker.SelectedIndex = -1;

            UserListView.ItemsSource = null;
            UserListView.ItemsSource = UserStore.RegisteredUsers;

            DisplayAlert("Success", "User created successfully!", "OK");
        }

        private void OnDeleteUserClicked(object sender, EventArgs e)
        {
            var selectedUser = UserListView.SelectedItem as User;
            if (selectedUser == null)
            {
                DisplayAlert("Error", "Please select a user to delete.", "OK");
                return;
            }

            UserStore.RegisteredUsers.Remove(selectedUser);

            UserListView.ItemsSource = null;
            UserListView.ItemsSource = UserStore.RegisteredUsers;

            DisplayAlert("Deleted", "User has been removed.", "OK");
        }

        private void OnFirmwareUpdateClicked(object sender, EventArgs e)
        {
            DisplayAlert("Firmware Update", "Sensor firmware update pushed successfully!", "OK");
        }
    }
}
