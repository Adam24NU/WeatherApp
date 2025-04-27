using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using WeatherApp.Repositories;
using WeatherApp.Tools;

namespace WeatherApp.ViewModels;

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

    [RelayCommand]
    public async Task Login()
    {
        if (string.IsNullOrWhiteSpace(Email) || string.IsNullOrWhiteSpace(Password))
        {
            StatusMessage = "Please enter both email and password.";
            return;
        }

        var user = await _userRepository.GetUserByEmailAsync(Email);
        if (user != null && await _userRepository.VerifyUserPasswordAsync(Email, Password))
        {
            StatusMessage = "Login successful!";
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
            StatusMessage = "Invalid email or password.";
        }
    }
}