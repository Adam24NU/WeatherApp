using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using WeatherApp.Models;
using WeatherApp.Repositories;
using WeatherApp.Tools;

namespace WeatherApp.ViewModels;

public partial class RegisterPageViewModel : ObservableObject
{
    private readonly UserRepository _userRepository;
    private readonly INavigationService _navigationService;

    [ObservableProperty]
    private string firstName;

    [ObservableProperty]
    private string lastName;

    [ObservableProperty]
    private string email;

    [ObservableProperty]
    private string password;

    [ObservableProperty]
    private string role;

    [ObservableProperty]
    private string statusMessage;

    [ObservableProperty]
    private ObservableCollection<string> availableRoles;

    public RegisterPageViewModel(UserRepository userRepository, INavigationService navigationService)
    {
        try
        {
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            _navigationService = navigationService;
            AvailableRoles = new ObservableCollection<string> { "Admin", "Scientist", "OpsManager" };
            Role = "Admin"; // Default role
        }
        catch (Exception ex)
        {
            StatusMessage = $"Initialization failed: {ex.Message}";
            System.Diagnostics.Debug.WriteLine($"RegisterPageViewModel init error: {ex}");
        }
    }

    [RelayCommand]
    public async Task Register()
    {
        try
        {
            System.Diagnostics.Debug.WriteLine($"Registering: {FirstName} {LastName}, {Email}, Role: {Role}");

            if (string.IsNullOrWhiteSpace(FirstName) ||
                string.IsNullOrWhiteSpace(LastName) ||
                string.IsNullOrWhiteSpace(Email) ||
                string.IsNullOrWhiteSpace(Password) ||
                string.IsNullOrWhiteSpace(Role))
            {
                StatusMessage = "Please fill all fields.";
                return;
            }

            var existingUser = await _userRepository.GetUserByEmailAsync(Email);
            if (existingUser != null)
            {
                StatusMessage = "Email already exists.";
                System.Diagnostics.Debug.WriteLine("Duplicate email detected.");
                return;
            }

            System.Diagnostics.Debug.WriteLine($"Password before hashing: {Password}");

            var newUser = new User
            {
                FirstName = FirstName,
                LastName = LastName,
                Email = Email,
                Password = Password,
                Role = Role
            };

            await _userRepository.InsertUserAsync(newUser);
            StatusMessage = "Registration successful!";
            await _navigationService.NavigateToAsync("LoginPage");
        }
        catch (Exception ex)
        {
            StatusMessage = $"Registration failed: {ex.Message}";
            System.Diagnostics.Debug.WriteLine($"Register error: {ex}");
        }
    }


    [RelayCommand]
    public async Task NavigateBack()
    {
        try
        {
            await Shell.Current.GoToAsync("..");
        }
        catch (Exception ex)
        {
            StatusMessage = $"Navigation failed: {ex.Message}";
            System.Diagnostics.Debug.WriteLine($"NavigateBack error: {ex}");
        }
    }
}