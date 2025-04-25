using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using WeatherApp.Repositories;
using System.Threading.Tasks;

namespace WeatherApp.ViewModels;

public partial class LoginPageViewModel : ObservableObject
{
    private readonly UserRepository _userRepository;

    [ObservableProperty]
    private string email;

    [ObservableProperty]
    private string password;

    [ObservableProperty]
    private string statusMessage;

    public LoginPageViewModel(UserRepository userRepository)
    {
        _userRepository = userRepository;
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
                    await Shell.Current.GoToAsync("AdminPage");
                    break;
                case "Scientist":
                    await Shell.Current.GoToAsync("ScientistPage");
                    break;
                case "OpsManager":
                    await Shell.Current.GoToAsync("OpsManagerPage");
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