using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using WeatherApp.Tools;

namespace WeatherApp.ViewModels;

public partial class MainPageViewModel : ObservableObject
{

    private readonly INavigationService _navigationService;
    private readonly IAlertService _alertService;

    public MainPageViewModel(INavigationService navigationService, IAlertService alertService)
    {
        _navigationService = navigationService;
        _alertService = alertService;
    }

    [RelayCommand]
    public async Task NavigateToLogin()
    {
        try
        {
            await _navigationService.NavigateToAsync("LoginPage");
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"NavigateToLogin error: {ex}");
            await _alertService.DisplayAlert("Error", $"Navigation failed: {ex.Message}", "OK");
        }
    }

    [RelayCommand]
    public async Task NavigateToRegister()
    {
        try
        {
            await _navigationService.NavigateToAsync("RegisterPage");
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"NavigateToRegister error: {ex}");
            await _alertService.DisplayAlert("Error", $"Navigation failed: {ex.Message}", "OK");
        }
    }
}