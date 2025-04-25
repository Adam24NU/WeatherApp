using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Threading.Tasks;

namespace WeatherApp.ViewModels;

public partial class MainPageViewModel : ObservableObject
{
    [RelayCommand]
    public async Task NavigateToLogin()
    {
        try
        {
            await Shell.Current.GoToAsync("LoginPage");
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"NavigateToLogin error: {ex}");
            await Shell.Current.DisplayAlert("Error", $"Navigation failed: {ex.Message}", "OK");
        }
    }

    [RelayCommand]
    private async Task NavigateToRegister()
    {
        try
        {
            await Shell.Current.GoToAsync("RegisterPage");
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"NavigateToRegister error: {ex}");
            await Shell.Current.DisplayAlert("Error", $"Navigation failed: {ex.Message}", "OK");
        }
    }
}