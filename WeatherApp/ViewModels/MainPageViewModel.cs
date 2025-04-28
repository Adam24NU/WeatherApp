using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using WeatherApp.Tools;

namespace WeatherApp.ViewModels;

/// <summary>
/// ViewModel for the MainPage.
/// Provides navigation commands to move to the Login and Register pages.
/// </summary>
public partial class MainPageViewModel : ObservableObject
{
    private readonly INavigationService _navigationService;
    private readonly IAlertService _alertService;

    /// <summary>
    /// Constructor to inject required services.
    /// </summary>
    /// <param name="navigationService">Service responsible for handling navigation between pages.</param>
    /// <param name="alertService">Service responsible for displaying alerts to the user.</param>
    public MainPageViewModel(INavigationService navigationService, IAlertService alertService)
    {
        _navigationService = navigationService;
        _alertService = alertService;
    }

    /// <summary>
    /// Command to navigate the user to the LoginPage.
    /// Handles and reports any navigation errors.
    /// </summary>
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

    /// <summary>
    /// Command to navigate the user to the RegisterPage.
    /// Handles and reports any navigation errors.
    /// </summary>
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
