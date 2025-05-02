namespace WeatherApp.Core.Tools
{
    public interface IAlertService
    {
        Task DisplayAlert(string title, string message, string cancel);
    }
}
