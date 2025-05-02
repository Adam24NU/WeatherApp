using Microsoft.Maui.Controls;
using WeatherApp.Core.Tools;

namespace WeatherApp.Tools;

public class AlertService : IAlertService
{
    public Task DisplayAlert(string title, string message, string cancel)
    {
        return Shell.Current.DisplayAlert(title, message, cancel);
    }
}