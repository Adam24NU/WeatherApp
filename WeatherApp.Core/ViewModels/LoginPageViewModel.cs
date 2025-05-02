using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using WeatherApp.Core.Repositories;
using WeatherApp.Core.Tools;

namespace WeatherApp.Core.ViewModels;

/// <summary>
/// ViewModel for handling user login functionality.
/// Manages user input, authentication, and navigation based on user roles.
/// </summary>
public partial class LoginPageViewModel : ObservableObject
{
    private readonly UserRepository _userRepository;
    private readonly INavigationService _navigationService;

    [ObservableProperty]
    private string email;
    [ObservableProperty]
    private string password;
    [ObservableProperty]
    private string statusMessage;

    public LoginPageViewModel(UserRepository userRepository, INavigationService navigationService)
    {
        _userRepository = userRepository;
        _navigationService = navigationService;
    }

    /// <summary>
    /// Command triggered when the user attempts to log in.
    /// Validates credentials, updates status messages, and navigates based on user role.
    /// </summary>
    [RelayCommand]
    public async Task Login()
    {
        // Ensure both fields are filled
        if (string.IsNullOrWhiteSpace(Email) || string.IsNullOrWhiteSpace(Password))
        {
            StatusMessage = "Please enter both email and password.";
            return;
        }

        // Attempt to retrieve user by email
        var user = await _userRepository.GetUserByEmailAsync(Email);

        // Validate user existence and password
        if (user != null && await _userRepository.VerifyUserPasswordAsync(Email, Password))
        {
            StatusMessage = "Login successful!";

            // Navigate to the appropriate page based on user role
            switch (user.Role)
            {
                case "Admin":
                    await _navigationService.NavigateToAsync("AdminPage");
                    break;
                case "Scientist":
                    await _navigationService.NavigateToAsync("ScientistPage");
                    break;
                case "OpsManager":
                    await _navigationService.NavigateToAsync("OpsManagerPage");
                    break;
                default:
                    StatusMessage = "Unknown role.";
                    break;
            }
        }
        else
        {
            // Credentials invalid
            StatusMessage = "Invalid email or password.";
        }
    }
}
