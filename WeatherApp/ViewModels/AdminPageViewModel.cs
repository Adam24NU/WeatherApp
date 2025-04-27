using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using WeatherApp.Models;
using WeatherApp.Repositories;
using WeatherApp.Tools;

namespace WeatherApp.ViewModels;

public partial class AdminPageViewModel : ObservableObject
{
    private readonly UserRepository _userRepository;
    private readonly IAlertService _alertService;

    [ObservableProperty]
    private ObservableCollection<User> registeredUsers;

    [ObservableProperty] private int userId;
    [ObservableProperty] private string firstName;
    [ObservableProperty] private string lastName;
    [ObservableProperty] private string email;
    [ObservableProperty] private string role;
    [ObservableProperty] private string password;

    [ObservableProperty]
    private User selectedUser;

    public AdminPageViewModel(UserRepository userRepository, IAlertService alertService)
    {
        _userRepository = userRepository;
        _alertService = alertService;
        RegisteredUsers = new ObservableCollection<User>();
    }

    public virtual async Task InitializeAsync()
    {
        try
        {
            await LoadUsersAsync();
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"InitializeAsync error: {ex}");
            await _alertService.DisplayAlert("Error", $"Failed to load users: {ex.Message}", "OK");
        }
    }

    public virtual async Task LoadUsersAsync()
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
    public virtual async Task CreateUser()
    {
        if (string.IsNullOrWhiteSpace(FirstName) || string.IsNullOrWhiteSpace(LastName) ||
            string.IsNullOrWhiteSpace(Email) || string.IsNullOrWhiteSpace(Role) ||
            string.IsNullOrWhiteSpace(Password))
        {
            await _alertService.DisplayAlert("Error", "Please fill out all fields.", "OK");
            return;
        }

        var existingUser = await _userRepository.GetUserByEmailAsync(Email.Trim());
        if (existingUser != null)
        {
            await _alertService.DisplayAlert("Error", "A user with this email already exists.", "OK");
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

        await _alertService.DisplayAlert("Success", "User created successfully!", "OK");
    }

    [RelayCommand]
    public virtual async Task DeleteUser()
    {
        if (SelectedUser == null)
        {
            await _alertService.DisplayAlert("Error", "Please select a user to delete.", "OK");
            return;
        }

        await _userRepository.DeleteUserAsync(SelectedUser.UserId);
        await LoadUsersAsync();

        await _alertService.DisplayAlert("Deleted", "User has been removed.", "OK");
    }

    [RelayCommand]
    private async Task SimulateFirmwareUpdate()
    {
        await _alertService.DisplayAlert("Firmware Update", "Sensor firmware update pushed successfully!", "OK");
    }
}