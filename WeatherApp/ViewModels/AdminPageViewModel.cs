using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using WeatherApp.Models;
using WeatherApp.Repositories;

namespace WeatherApp.ViewModels;

public partial class AdminPageViewModel : ObservableObject
{
    private readonly UserRepository _userRepository;

    [ObservableProperty]
    private ObservableCollection<User> registeredUsers;

    [ObservableProperty] private string firstName;
    [ObservableProperty] private string lastName;
    [ObservableProperty] private string email;
    [ObservableProperty] private string role;
    [ObservableProperty] private string password;

    [ObservableProperty]
    private User selectedUser;

    public AdminPageViewModel()
    {
        _userRepository = new UserRepository(new DatabaseConnection());
        RegisteredUsers = new ObservableCollection<User>(); 
    }

    public async Task InitializeAsync()
    {
        try
        {
            await LoadUsersAsync();
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"InitializeAsync error: {ex}");
            await Shell.Current.DisplayAlert("Error", $"Failed to load users: {ex.Message}", "OK");
        }
    }

    private async Task LoadUsersAsync()
    {
        try
        {
            var users = await _userRepository.GetUsersAsync();
            RegisteredUsers = new ObservableCollection<User>(users);
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"LoadUsersAsync error: {ex}");
            throw;
        }
    }

    [RelayCommand]
    private async Task CreateUser()
    {
        if (string.IsNullOrWhiteSpace(FirstName) || string.IsNullOrWhiteSpace(LastName) ||
            string.IsNullOrWhiteSpace(Email) || string.IsNullOrWhiteSpace(Role) ||
            string.IsNullOrWhiteSpace(Password))
        {
            await Shell.Current.DisplayAlert("Error", "Please fill out all fields.", "OK");
            return;
        }

        var existingUser = await _userRepository.GetUserByEmailAsync(Email.Trim());
        if (existingUser != null)
        {
            await Shell.Current.DisplayAlert("Error", "A user with this email already exists.", "OK");
            return;
        }

        var newUser = new User
        {
            FirstName = FirstName.Trim(),
            LastName = LastName.Trim(),
            Email = Email.Trim(),
            Role = Role,
            Password = Password
        };

        await _userRepository.InsertUserAsync(newUser);
        await LoadUsersAsync();

        FirstName = LastName = Email = Password = string.Empty;
        Role = null;

        await Shell.Current.DisplayAlert("Success", "User created successfully!", "OK");
    }

    [RelayCommand]
    private async Task DeleteUser()
    {
        if (SelectedUser == null)
        {
            await Shell.Current.DisplayAlert("Error", "Please select a user to delete.", "OK");
            return;
        }

        await _userRepository.DeleteUserAsync(SelectedUser.UserId);
        await LoadUsersAsync();

        await Shell.Current.DisplayAlert("Deleted", "User has been removed.", "OK");
    }

    [RelayCommand]
    private async Task SimulateFirmwareUpdate()
    {
        await Shell.Current.DisplayAlert("Firmware Update", "Sensor firmware update pushed successfully!", "OK");
    }
}